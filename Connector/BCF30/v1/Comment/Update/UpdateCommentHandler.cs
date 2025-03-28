using Connector.Client;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Client.AppNetwork;
using Connector.BCF30.v1.Comment.Models;

namespace Connector.BCF30.v1.Comment.Update;

public class UpdateCommentHandler : IActionHandler<UpdateCommentAction>
{
    private readonly ILogger<UpdateCommentHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateCommentHandler(
        ILogger<UpdateCommentHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateCommentActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateCommentHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.UpdateBcf30Comment(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                commentId: input.CommentId,
                comment: input.Comment,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful || response.Data == null)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(UpdateCommentHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to update comment"
                        }
                    }
                });
            }

            // Build sync operations to update the cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(response.Data);
            operations.Add(SyncOperation.CreateSyncOperation("Upsert", key.UrlPart, key.PropertyNames, response.Data));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(CommentDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while updating BCF 3.0 comment for project {ProjectId}, topic {TopicId}, and comment {CommentId}",
                input.ProjectId, input.TopicId, input.CommentId);

            var errorSource = new List<string> { nameof(UpdateCommentHandler) };
            if (!string.IsNullOrEmpty(exception.Source)) errorSource.Add(exception.Source);
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = errorSource.ToArray(),
                        Text = exception.Message
                    }
                }
            });
        }
    }
}

using Connector.BCF30.v1.Topics;
using Connector.Client;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Client.AppNetwork;

namespace Connector.BCF30.v1.Topic.Delete;

public class DeleteTopicHandler : IActionHandler<DeleteTopicAction>
{
    private readonly ILogger<DeleteTopicHandler> _logger;
    private readonly ApiClient _apiClient;

    public DeleteTopicHandler(
        ILogger<DeleteTopicHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<DeleteTopicActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(DeleteTopicHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.DeleteBcf30Topic(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(DeleteTopicHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to delete topic"
                        }
                    }
                });
            }

            // Build sync operations to update the cache
            var operations = new List<SyncOperation>();
            var key = new DataObjectKey($"bcf/3.0/projects/{input.ProjectId}/topics/{input.TopicId}", Array.Empty<string>(), false);
            operations.Add(SyncOperation.CreateSyncOperation("Delete", key.UrlPart, key.PropertyNames, null));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(TopicsDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(new DeleteTopicActionOutput(), resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while deleting BCF 3.0 topic {TopicId} for project {ProjectId}",
                input.TopicId, input.ProjectId);

            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(DeleteTopicHandler) },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}

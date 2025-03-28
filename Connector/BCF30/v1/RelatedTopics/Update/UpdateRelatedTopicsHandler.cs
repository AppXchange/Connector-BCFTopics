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

namespace Connector.BCF30.v1.RelatedTopics.Update;

public class UpdateRelatedTopicsHandler : IActionHandler<UpdateRelatedTopicsAction>
{
    private readonly ILogger<UpdateRelatedTopicsHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateRelatedTopicsHandler(
        ILogger<UpdateRelatedTopicsHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateRelatedTopicsActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateRelatedTopicsHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.UpdateBcf30RelatedTopics(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                relatedTopics: input.RelatedTopics,
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
                            Source = new[] { nameof(UpdateRelatedTopicsHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to update related topics"
                        }
                    }
                });
            }

            // Build sync operations to update the cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();

            foreach (var topic in response.Data)
            {
                var key = keyResolver.BuildKeyResolver()(topic);
                operations.Add(SyncOperation.CreateSyncOperation("Upsert", key.UrlPart, key.PropertyNames, topic));
            }

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(RelatedTopicsDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(new UpdateRelatedTopicsActionOutput { RelatedTopics = response.Data }, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while updating BCF 3.0 related topics for project {ProjectId} and topic {TopicId}",
                input.ProjectId, input.TopicId);

            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateRelatedTopicsHandler) },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}

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

namespace Connector.BCF30.v1.Viewpoint.Delete;

public class DeleteViewpointHandler : IActionHandler<DeleteViewpointAction>
{
    private readonly ILogger<DeleteViewpointHandler> _logger;
    private readonly ApiClient _apiClient;

    public DeleteViewpointHandler(
        ILogger<DeleteViewpointHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<DeleteViewpointActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(DeleteViewpointHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.DeleteBcf30Viewpoint(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                viewpointId: input.ViewpointId,
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
                            Source = new[] { nameof(DeleteViewpointHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to delete viewpoint"
                        }
                    }
                });
            }

            // Build sync operations to update the cache
            var operations = new List<SyncOperation>();
            var key = new DataObjectKey($"bcf/3.0/projects/{input.ProjectId}/topics/{input.TopicId}/viewpoints/{input.ViewpointId}", Array.Empty<string>(), false);
            operations.Add(SyncOperation.CreateSyncOperation("Delete", key.UrlPart, key.PropertyNames, null));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(ViewpointDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(new DeleteViewpointActionOutput(), resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while deleting BCF 3.0 viewpoint {ViewpointId} for project {ProjectId} and topic {TopicId}",
                input.ViewpointId, input.ProjectId, input.TopicId);

            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(DeleteViewpointHandler) },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}

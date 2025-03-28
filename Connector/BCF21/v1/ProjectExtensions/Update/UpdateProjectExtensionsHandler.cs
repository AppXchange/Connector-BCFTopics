using Connector.Client;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.ProjectExtensions.Update;

public class UpdateProjectExtensionsHandler : IActionHandler<UpdateProjectExtensionsAction>
{
    private readonly ILogger<UpdateProjectExtensionsHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateProjectExtensionsHandler(
        ILogger<UpdateProjectExtensionsHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateProjectExtensionsActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateProjectExtensionsHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.UpdateBcf21ProjectExtensions(
                input.ProjectId,
                new
                {
                    snippet_type = input.SnippetType,
                    topic_type_full = input.TopicTypeFull,
                    topic_status_full = input.TopicStatusFull,
                    topic_label_full = input.TopicLabelFull,
                    priority_full = input.PriorityFull,
                    stage_full = input.StageFull,
                    topic_type = input.TopicType,
                    topic_status = input.TopicStatus,
                    topic_label = input.TopicLabel,
                    priority = input.Priority,
                    stage = input.Stage
                },
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful || response.Data == null)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(UpdateProjectExtensionsHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } ? 
                                await new System.IO.StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) : 
                                "Failed to update project extensions"
                        }
                    }
                });
            }

            // Build sync operations to update the local cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(response.Data);
            operations.Add(SyncOperation.CreateSyncOperation("Upsert", key.UrlPart, key.PropertyNames, response.Data));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(ProjectExtensionsDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            var errorSource = new List<string> { nameof(UpdateProjectExtensionsHandler) };
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

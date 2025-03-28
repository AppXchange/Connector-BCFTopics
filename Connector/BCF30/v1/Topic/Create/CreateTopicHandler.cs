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
using System;
using System.Linq;

namespace Connector.BCF30.v1.Topic.Create;

public class CreateTopicHandler : IActionHandler<CreateTopicAction>
{
    private readonly ILogger<CreateTopicHandler> _logger;
    private readonly ApiClient _apiClient;

    public CreateTopicHandler(
        ILogger<CreateTopicHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<CreateTopicActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(CreateTopicHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.CreateBcf30Topic(
                projectId: input.ProjectId,
                topicType: input.TopicType,
                topicStatus: input.TopicStatus,
                title: input.Title,
                priority: input.Priority,
                labels: input.Labels ?? Enumerable.Empty<string>(),
                assignedTo: input.AssignedTo,
                bimSnippet: input.BimSnippet is null ? null : new Update.BimSnippetInput 
                {
                    SnippetType = input.BimSnippet.SnippetType,
                    IsExternal = input.BimSnippet.IsExternal,
                    Reference = input.BimSnippet.Reference,
                    ReferenceSchema = input.BimSnippet.ReferenceSchema
                },
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
                            Source = new[] { nameof(CreateTopicHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to create topic"
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
                new() { DataObjectType = typeof(TopicDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while creating BCF 3.0 topic for project {ProjectId}",
                input.ProjectId);

            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(CreateTopicHandler) },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}

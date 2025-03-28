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

namespace Connector.BCF21.v1.Topic.Update;

internal static class DataObjectExtensions
{
    public static bool TryGetParameterValue<T>(this ActionInstance actionInstance, string key, out T? value)
    {
        value = default;
        if (actionInstance == null) return false;

        var dict = actionInstance.GetType().GetProperty("Parameters")?.GetValue(actionInstance) as IDictionary<string, object>;
        if (dict == null || !dict.ContainsKey(key)) return false;

        try
        {
            value = (T)dict[key];
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class UpdateTopicHandler : IActionHandler<UpdateTopicAction>
{
    private readonly ILogger<UpdateTopicHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateTopicHandler(
        ILogger<UpdateTopicHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateTopicActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateTopicHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            if (!actionInstance.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = "400",
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(UpdateTopicHandler) },
                            Text = "Project ID is required"
                        }
                    }
                });
            }

            if (!actionInstance.TryGetParameterValue("topic_id", out string? topicId) || string.IsNullOrEmpty(topicId))
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = "400",
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(UpdateTopicHandler) },
                            Text = "Topic ID is required"
                        }
                    }
                });
            }

            var response = await _apiClient.UpdateBcf21Topic(
                projectId,
                topicId,
                input,
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
                            Source = new[] { nameof(UpdateTopicHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } ? 
                                await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) : 
                                "Request to target system failed"
                        }
                    }
                });
            }

            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(response.Data);
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, response.Data));

            var resultList = new List<CacheSyncCollection>
            {
                new CacheSyncCollection() { DataObjectType = typeof(TopicDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            var errorSource = new List<string> { nameof(UpdateTopicHandler) };
            if (string.IsNullOrEmpty(exception.Source)) errorSource.Add(exception.Source!);
            
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

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
using System.Linq;

namespace Connector.BCF21.v1.RelatedTopics.Update;

public class UpdateRelatedTopicsHandler : IActionHandler<UpdateRelatedTopicsAction>
{
    private readonly ILogger<UpdateRelatedTopicsHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateRelatedTopicsHandler(
        ILogger<UpdateRelatedTopicsHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new System.ArgumentNullException(nameof(apiClient));
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateRelatedTopicsActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Error { Text = "Invalid input data" } }
            });
        }

        try
        {
            var response = await _apiClient.UpdateBcf21RelatedTopics(
                input.ProjectId,
                input.TopicId,
                input.RelatedTopics,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful || response.Data == null)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[] { new Error { Text = "Failed to update related topics" } }
                });
            }

            var operations = new List<SyncOperation>();
            foreach (var relatedTopic in response.Data)
            {
                var keyResolver = new DefaultDataObjectKey();
                var key = keyResolver.BuildKeyResolver()(relatedTopic);
                operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, relatedTopic));
            }

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(RelatedTopicsDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error updating related topics");
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = ex.StatusCode?.ToString() ?? "500",
                Errors = new[] { new Error { Text = ex.Message } }
            });
        }
    }
}

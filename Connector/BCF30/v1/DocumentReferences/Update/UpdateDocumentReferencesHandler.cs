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
using Connector.BCF30.v1.DocumentReferences.Models;

namespace Connector.BCF30.v1.DocumentReferences.Update;

public class UpdateDocumentReferencesHandler : IActionHandler<UpdateDocumentReferencesAction>
{
    private readonly ILogger<UpdateDocumentReferencesHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateDocumentReferencesHandler(
        ILogger<UpdateDocumentReferencesHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateDocumentReferencesActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateDocumentReferencesHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.UpdateBcf30DocumentReference(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                documentReferenceId: input.DocumentReferenceId,
                url: input.Url,
                documentGuid: input.DocumentGuid,
                description: input.Description,
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
                            Source = new[] { nameof(UpdateDocumentReferencesHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to update document reference"
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
                new() { DataObjectType = typeof(DocumentReferencesDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while updating BCF 3.0 document reference for project {ProjectId}, topic {TopicId}, and reference {DocumentReferenceId}",
                input.ProjectId, input.TopicId, input.DocumentReferenceId);

            var errorSource = new List<string> { nameof(UpdateDocumentReferencesHandler) };
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

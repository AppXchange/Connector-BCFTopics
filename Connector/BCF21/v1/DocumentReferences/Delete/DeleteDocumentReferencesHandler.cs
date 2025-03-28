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

namespace Connector.BCF21.v1.DocumentReferences.Delete;

public class DeleteDocumentReferencesHandler : IActionHandler<DeleteDocumentReferencesAction>
{
    private readonly ILogger<DeleteDocumentReferencesHandler> _logger;
    private readonly ApiClient _apiClient;

    public DeleteDocumentReferencesHandler(
        ILogger<DeleteDocumentReferencesHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<DeleteDocumentReferencesActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(DeleteDocumentReferencesHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.DeleteBcf21DocumentReference(
                input.ProjectId,
                input.TopicId,
                input.DocumentReferenceId,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(DeleteDocumentReferencesHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } ? 
                                await new System.IO.StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) : 
                                "Failed to delete document reference"
                        }
                    }
                });
            }

            // For delete operations, we need to remove the item from cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(new DeleteDocumentReferencesActionOutput());
            operations.Add(SyncOperation.CreateSyncOperation("Delete", key.UrlPart, key.PropertyNames, new DeleteDocumentReferencesActionOutput()));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(DocumentReferencesDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(new DeleteDocumentReferencesActionOutput(), resultList);
        }
        catch (HttpRequestException exception)
        {
            var errorSource = new List<string> { nameof(DeleteDocumentReferencesHandler) };
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

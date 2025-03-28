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

namespace Connector.BCF21.v1.DocumentReferences.Create;

public class CreateDocumentReferencesHandler : IActionHandler<CreateDocumentReferencesAction>
{
    private readonly ILogger<CreateDocumentReferencesHandler> _logger;
    private readonly ApiClient _apiClient;

    public CreateDocumentReferencesHandler(
        ILogger<CreateDocumentReferencesHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<CreateDocumentReferencesActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(CreateDocumentReferencesHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        if (string.IsNullOrEmpty(input.DocumentGuid) && string.IsNullOrEmpty(input.Url))
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(CreateDocumentReferencesHandler) },
                        Text = "Either document_guid or url must be provided"
                    }
                }
            });
        }

        if (!string.IsNullOrEmpty(input.DocumentGuid) && !string.IsNullOrEmpty(input.Url))
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(CreateDocumentReferencesHandler) },
                        Text = "document_guid and url are mutually exclusive"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.CreateBcf21DocumentReference(
                input.ProjectId,
                input.TopicId,
                new
                {
                    input.Guid,
                    input.DocumentGuid,
                    input.Url,
                    input.Description
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
                            Source = new[] { nameof(CreateDocumentReferencesHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } ? 
                                await new System.IO.StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) : 
                                "Failed to create document reference"
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
                new() { DataObjectType = typeof(DocumentReferencesDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            var errorSource = new List<string> { nameof(CreateDocumentReferencesHandler) };
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

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

namespace Connector.BCF21.v1.Files.Update;

public class UpdateFilesHandler : IActionHandler<UpdateFilesAction>
{
    private readonly ILogger<UpdateFilesHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateFilesHandler(
        ILogger<UpdateFilesHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UpdateFilesActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(UpdateFilesHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.UpdateBcf21TopicFiles(
                input.ProjectId,
                input.TopicId,
                input.Files,
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
                            Source = new[] { nameof(UpdateFilesHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } ? 
                                await new System.IO.StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) : 
                                "Failed to update topic files"
                        }
                    }
                });
            }

            // Build sync operations to update the local cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            foreach (var file in response.Data)
            {
                var key = keyResolver.BuildKeyResolver()(file);
                operations.Add(SyncOperation.CreateSyncOperation("Upsert", key.UrlPart, key.PropertyNames, file));
            }

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(FilesDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(new UpdateFilesActionOutput { Files = response.Data }, resultList);
        }
        catch (HttpRequestException exception)
        {
            var errorSource = new List<string> { nameof(UpdateFilesHandler) };
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

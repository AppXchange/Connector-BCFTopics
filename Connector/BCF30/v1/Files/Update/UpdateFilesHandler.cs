using Connector.Client;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Client.AppNetwork;

namespace Connector.BCF30.v1.Files.Update;

public class UpdateFilesHandler : IActionHandler<UpdateFilesAction>
{
    private readonly ILogger<UpdateFilesHandler> _logger;
    private readonly ApiClient _apiClient;

    public UpdateFilesHandler(
        ILogger<UpdateFilesHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
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
            var response = await _apiClient.UpdateBcf30Files(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                files: input.Files,
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
                            Source = new[] { nameof(UpdateFilesHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to update files"
                        }
                    }
                });
            }

            // Build sync operations to update the cache
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

            var outputFiles = response.Data.Select(file => new FileReference
            {
                IfcProject = file.IfcProject,
                Filename = file.Filename,
                Reference = file.Reference
            });

            return ActionHandlerOutcome.Successful(new UpdateFilesActionOutput { Files = outputFiles }, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while updating BCF 3.0 files for project {ProjectId} and topic {TopicId}",
                input.ProjectId, input.TopicId);

            var errorSource = new[] { nameof(UpdateFilesHandler) };
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = errorSource,
                        Text = exception.Message
                    }
                }
            });
        }
    }
}

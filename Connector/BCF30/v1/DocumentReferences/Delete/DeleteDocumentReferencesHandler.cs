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

namespace Connector.BCF30.v1.DocumentReferences.Delete;

public class DeleteDocumentReferencesHandler : IActionHandler<DeleteDocumentReferencesAction>
{
    private readonly ILogger<DeleteDocumentReferencesHandler> _logger;
    private readonly ApiClient _apiClient;

    public DeleteDocumentReferencesHandler(
        ILogger<DeleteDocumentReferencesHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
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
            var response = await _apiClient.DeleteBcf30DocumentReference(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                documentReferenceId: input.DocumentReferenceId,
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
                            Source = new[] { nameof(DeleteDocumentReferencesHandler) },
                            Text = "Failed to delete document reference"
                        }
                    }
                });
            }

            // For delete operations, we return an empty output since the API returns 204 No Content
            return ActionHandlerOutcome.Successful(new DeleteDocumentReferencesActionOutput());
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while deleting BCF 3.0 document reference for project {ProjectId}, topic {TopicId}, and reference {DocumentReferenceId}",
                input.ProjectId, input.TopicId, input.DocumentReferenceId);

            var errorSource = new[] { nameof(DeleteDocumentReferencesHandler) };
            
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

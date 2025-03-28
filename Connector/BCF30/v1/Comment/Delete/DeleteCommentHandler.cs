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

namespace Connector.BCF30.v1.Comment.Delete;

public class DeleteCommentHandler : IActionHandler<DeleteCommentAction>
{
    private readonly ILogger<DeleteCommentHandler> _logger;
    private readonly ApiClient _apiClient;

    public DeleteCommentHandler(
        ILogger<DeleteCommentHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<DeleteCommentActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Error
                    {
                        Source = new[] { nameof(DeleteCommentHandler) },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.DeleteBcf30Comment(
                projectId: input.ProjectId,
                topicId: input.TopicId,
                commentId: input.CommentId,
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
                            Source = new[] { nameof(DeleteCommentHandler) },
                            Text = response.RawResult is { Position: 0, Length: > 0 } 
                                ? await new StreamReader(response.RawResult).ReadToEndAsync(cancellationToken) 
                                : "Failed to delete comment"
                        }
                    }
                });
            }

            // Return success with no output since it's a delete operation
            return ActionHandlerOutcome.Successful(new DeleteCommentActionOutput());
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while deleting BCF 3.0 comment for project {ProjectId}, topic {TopicId}, and comment {CommentId}",
                input.ProjectId, input.TopicId, input.CommentId);

            var errorSource = new List<string> { nameof(DeleteCommentHandler) };
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

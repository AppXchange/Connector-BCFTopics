using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;
using Connector.BCF30.v1.Comment.Models;

namespace Connector.BCF30.v1.Comment;

public class CommentDataReader : TypedAsyncDataReaderBase<CommentDataObject>
{
    private readonly ILogger<CommentDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _topicId;
    private readonly string _commentId;

    public CommentDataReader(
        ILogger<CommentDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string topicId,
        string commentId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
        _topicId = topicId;
        _commentId = commentId;
    }

    public override async IAsyncEnumerable<CommentDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        CommentDataObject? comment = null;
        try
        {
            var response = await _apiClient.GetBcf30Comment(
                projectId: _projectId,
                topicId: _topicId,
                commentId: _commentId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiException
                {
                    StatusCode = response.StatusCode,
                    Content = response.RawResult
                };
            }

            comment = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception,
                "Exception while making a read request to BCF 3.0 Comment endpoint for project {ProjectId}, topic {TopicId}, and comment {CommentId}",
                _projectId, _topicId, _commentId);
            throw;
        }

        if (comment != null)
        {
            yield return comment;
        }
    }
}
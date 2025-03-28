using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;
using Connector.BCF30.v1.Comments.Models;

namespace Connector.BCF30.v1.Comments;

public class CommentsDataReader : TypedAsyncDataReaderBase<CommentsDataObject>
{
    private readonly ILogger<CommentsDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _topicId;

    public CommentsDataReader(
        ILogger<CommentsDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string topicId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
        _topicId = topicId;
    }

    public override async IAsyncEnumerable<CommentsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<CommentsDataObject>? comments = null;
        try
        {
            var response = await _apiClient.GetBcf30Comments(
                projectId: _projectId,
                topicId: _topicId,
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

            comments = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to BCF Comments endpoint for project {ProjectId} and topic {TopicId}", 
                _projectId, _topicId);
            throw;
        }

        if (comments != null)
        {
            foreach (var comment in comments)
            {
                yield return comment;
            }
        }
    }
}
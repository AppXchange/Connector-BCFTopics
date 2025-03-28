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
using Connector.BCF21.v1.Comments.Models;

namespace Connector.BCF21.v1.Comments;

public class CommentsDataReader : TypedAsyncDataReaderBase<CommentsDataObject>
{
    private readonly ILogger<CommentsDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _topicId;
    private readonly int _pageSize;
    private string? _skipToken;

    public CommentsDataReader(
        ILogger<CommentsDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string topicId,
        int pageSize = 500)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
        _topicId = topicId;
        _pageSize = Math.Min(pageSize, 500); // Ensure we don't exceed max page size
    }

    public override async IAsyncEnumerable<CommentsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        bool hasMoreData = true;

        while (hasMoreData)
        {
            ApiResponse<IEnumerable<CommentsDataObject>>? response = null;
            IEnumerable<CommentsDataObject>? currentPageComments = null;

            try
            {
                response = await _apiClient.GetBcf21Comments(
                    projectId: _projectId,
                    topicId: _topicId,
                    top: _pageSize,
                    skipToken: _skipToken,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                if (!response.IsSuccessful)
                {
                    if (response.StatusCode == 204)
                    {
                        // No more comments to retrieve
                        hasMoreData = false;
                        continue;
                    }

                    throw new ApiException
                    {
                        StatusCode = response.StatusCode,
                        Content = response.RawResult
                    };
                }

                currentPageComments = response.Data;

                // Check if we got any data
                if (currentPageComments == null || !currentPageComments.Any())
                {
                    hasMoreData = false;
                    continue;
                }

                // Update pagination token from response
                var nextLink = response.Headers?.GetValues("odata.nextLink").FirstOrDefault();
                if (string.IsNullOrEmpty(nextLink))
                {
                    hasMoreData = false;
                }
                else
                {
                    // Extract skiptoken from the nextLink
                    var uri = new Uri(nextLink);
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    _skipToken = query["skiptoken"];
                    hasMoreData = !string.IsNullOrEmpty(_skipToken);
                }
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, 
                    "Exception while making a read request to BCF 2.1 Comments endpoint for project {ProjectId} and topic {TopicId}", 
                    _projectId, _topicId);
                throw;
            }

            // Return the current page of comments outside the try block
            if (currentPageComments != null)
            {
                foreach (var comment in currentPageComments)
                {
                    yield return comment;
                }
            }
        }
    }
}
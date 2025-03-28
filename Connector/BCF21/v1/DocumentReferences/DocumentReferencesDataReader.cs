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

namespace Connector.BCF21.v1.DocumentReferences;

public class DocumentReferencesDataReader : TypedAsyncDataReaderBase<DocumentReferencesDataObject>
{
    private readonly ILogger<DocumentReferencesDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _topicId;
    private string? _skipToken;

    public DocumentReferencesDataReader(
        ILogger<DocumentReferencesDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string topicId)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        _topicId = topicId ?? throw new ArgumentNullException(nameof(topicId));
    }

    public override async IAsyncEnumerable<DocumentReferencesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const int pageSize = 500; // Maximum page size as per API documentation
        bool hasMoreData;

        do
        {
            ApiResponse<IEnumerable<DocumentReferencesDataObject>>? response = null;

            try
            {
                response = await _apiClient.GetBcf21DocumentReferences(
                    _projectId,
                    _topicId,
                    pageSize,
                    _skipToken,
                    cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessful)
                {
                    _logger.LogError("Failed to retrieve document references for project {ProjectId} and topic {TopicId}. API StatusCode: {StatusCode}", 
                        _projectId, _topicId, response.StatusCode);
                    throw new Exception($"Failed to retrieve document references. API StatusCode: {response.StatusCode}");
                }
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while making a read request to data object 'DocumentReferencesDataObject'");
                throw;
            }

            if (response?.Data == null)
            {
                yield break;
            }

            foreach (var documentReference in response.Data)
            {
                yield return documentReference;
            }

            // Update skipToken for next page if available in headers
            _skipToken = response?.Headers?.TryGetValues("X-Skip-Token", out var values) == true ? 
                values.FirstOrDefault() : 
                null;

            hasMoreData = !string.IsNullOrEmpty(_skipToken);
        }
        while (hasMoreData);
    }
}
using Connector.Client;
using Connector.BCF30.v1.DocumentReferences.Models;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.BCF30.v1.DocumentReferences;

public class DocumentReferencesDataReader : TypedAsyncDataReaderBase<DocumentReferencesDataObject>
{
    private readonly ILogger<DocumentReferencesDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _topicId;

    public DocumentReferencesDataReader(
        ILogger<DocumentReferencesDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string topicId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
        _topicId = topicId;
    }

    public override async IAsyncEnumerable<DocumentReferencesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<DocumentReferencesDataObject>? documentReferences = null;

        try
        {
            var response = await _apiClient.GetBcf30DocumentReferences(
                projectId: _projectId,
                topicId: _topicId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 document references. API StatusCode: {response.StatusCode}");
            }

            documentReferences = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 document references for project {ProjectId} and topic {TopicId}", 
                _projectId, _topicId);
            throw;
        }

        if (documentReferences != null)
        {
            foreach (var documentReference in documentReferences)
            {
                yield return documentReference;
            }
        }
    }
}
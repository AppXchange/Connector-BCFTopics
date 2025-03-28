using Connector.Client;
using Connector.BCF30.v1.Document.Models;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.BCF30.v1.Document;

public class DocumentDataReader : TypedAsyncDataReaderBase<DocumentDataObject>
{
    private readonly ILogger<DocumentDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _documentId;

    public DocumentDataReader(
        ILogger<DocumentDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string documentId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
        _documentId = documentId;
    }

    public override async IAsyncEnumerable<DocumentDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        DocumentDataObject? document = null;

        try
        {
            var response = await _apiClient.GetBcf30Document(
                projectId: _projectId,
                documentId: _documentId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 document. API StatusCode: {response.StatusCode}");
            }

            document = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 document for project {ProjectId} and document {DocumentId}", 
                _projectId, _documentId);
            throw;
        }

        if (document != null)
        {
            yield return document;
        }
    }
}
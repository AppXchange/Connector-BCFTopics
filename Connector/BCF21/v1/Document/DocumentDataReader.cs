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

namespace Connector.BCF21.v1.Document;

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
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        _documentId = documentId ?? throw new ArgumentNullException(nameof(documentId));
    }

    public override async IAsyncEnumerable<DocumentDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<DocumentDataObject>? response = null;

        try
        {
            response = await _apiClient.GetBcf21Document(
                _projectId,
                _documentId,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve document {DocumentId} for project {ProjectId}. API StatusCode: {StatusCode}", 
                    _documentId, _projectId, response.StatusCode);
                throw new Exception($"Failed to retrieve document. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'DocumentDataObject'");
            throw;
        }

        if (response?.Data == null)
        {
            yield break;
        }

        yield return response.Data;
    }
}
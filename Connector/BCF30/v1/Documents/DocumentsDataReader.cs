using Connector.Client;
using Connector.BCF30.v1.Documents.Models;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.BCF30.v1.Documents;

public class DocumentsDataReader : TypedAsyncDataReaderBase<DocumentsDataObject>
{
    private readonly ILogger<DocumentsDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;

    public DocumentsDataReader(
        ILogger<DocumentsDataReader> logger,
        ApiClient apiClient,
        string projectId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
    }

    public override async IAsyncEnumerable<DocumentsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<DocumentsDataObject>? documents = null;

        try
        {
            var response = await _apiClient.GetBcf30Documents(
                projectId: _projectId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 documents. API StatusCode: {response.StatusCode}");
            }

            documents = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving BCF 3.0 documents for project {ProjectId}", _projectId);
            throw;
        }

        if (documents != null)
        {
            foreach (var document in documents)
            {
                yield return document;
            }
        }
    }
}
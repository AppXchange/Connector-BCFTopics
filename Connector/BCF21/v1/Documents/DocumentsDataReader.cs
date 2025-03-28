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

namespace Connector.BCF21.v1.Documents;

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
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
    }

    public override async IAsyncEnumerable<DocumentsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<IEnumerable<DocumentsDataObject>>? response = null;

        try
        {
            response = await _apiClient.GetBcf21Documents(
                _projectId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve documents for project {ProjectId}. API StatusCode: {StatusCode}", 
                    _projectId, response.StatusCode);
                throw new Exception($"Failed to retrieve documents. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'DocumentsDataObject'");
            throw;
        }

        if (response?.Data == null)
        {
            yield break;
        }

        foreach (var document in response.Data)
        {
            yield return document;
        }
    }
}
using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF30.v1.FilesInfo;

public class FilesInfoDataReader : TypedAsyncDataReaderBase<FilesInfoDataObject>
{
    private readonly ILogger<FilesInfoDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;

    public FilesInfoDataReader(
        ILogger<FilesInfoDataReader> logger,
        ApiClient apiClient,
        string projectId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
    }

    public override async IAsyncEnumerable<FilesInfoDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<FilesInfoDataObject>? filesInfo = null;

        try
        {
            var response = await _apiClient.GetBcf30FilesInformation(
                projectId: _projectId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 files information. API StatusCode: {response.StatusCode}");
            }

            filesInfo = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 files information for project {ProjectId}", 
                _projectId);
            throw;
        }

        if (filesInfo != null)
        {
            foreach (var fileInfo in filesInfo)
            {
                yield return fileInfo;
            }
        }
    }
}
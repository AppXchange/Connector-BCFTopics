using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF30.v1.Files;

public class FilesDataReader : TypedAsyncDataReaderBase<FilesDataObject>
{
    private readonly ILogger<FilesDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly string _topicId;

    public FilesDataReader(
        ILogger<FilesDataReader> logger,
        ApiClient apiClient,
        string projectId,
        string topicId)
    {
        _logger = logger;
        _apiClient = apiClient;
        _projectId = projectId;
        _topicId = topicId;
    }

    public override async IAsyncEnumerable<FilesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<FilesDataObject>? files = null;

        try
        {
            var response = await _apiClient.GetBcf30Files(
                projectId: _projectId,
                topicId: _topicId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 files. API StatusCode: {response.StatusCode}");
            }

            files = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 files for project {ProjectId} and topic {TopicId}", 
                _projectId, _topicId);
            throw;
        }

        if (files != null)
        {
            foreach (var file in files)
            {
                yield return file;
            }
        }
    }
}
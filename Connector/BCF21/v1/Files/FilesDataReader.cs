using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.Files;

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
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        _topicId = topicId ?? throw new ArgumentNullException(nameof(topicId));
    }

    public override async IAsyncEnumerable<FilesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<FilesDataObject>? files = null;
        try
        {
            var response = await _apiClient.GetBcf21TopicFiles(
                _projectId,
                _topicId,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve files for topic. Status code: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve files. API StatusCode: {response.StatusCode}");
            }

            files = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving files for topic {TopicId}", _topicId);
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
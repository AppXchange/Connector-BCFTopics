using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.Project;

public class ProjectDataReader : TypedAsyncDataReaderBase<ProjectDataObject>
{
    private readonly ILogger<ProjectDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;

    public ProjectDataReader(
        ILogger<ProjectDataReader> logger,
        ApiClient apiClient,
        string projectId)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
    }

    public override async IAsyncEnumerable<ProjectDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ProjectDataObject? project = null;
        try
        {
            var response = await _apiClient.GetBcf21Project(
                _projectId,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve project {ProjectId}. Status code: {StatusCode}", _projectId, response.StatusCode);
                throw new Exception($"Failed to retrieve project. API StatusCode: {response.StatusCode}");
            }

            project = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving project {ProjectId}", _projectId);
            throw;
        }

        if (project != null)
        {
            yield return project;
        }
    }
}
using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.ProjectExtensions;

public class ProjectExtensionsDataReader : TypedAsyncDataReaderBase<ProjectExtensionsDataObject>
{
    private readonly ILogger<ProjectExtensionsDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly string _projectId;
    private readonly bool _includeUsers;

    public ProjectExtensionsDataReader(
        ILogger<ProjectExtensionsDataReader> logger,
        ApiClient apiClient,
        string projectId,
        bool includeUsers = true)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        _includeUsers = includeUsers;
    }

    public override async IAsyncEnumerable<ProjectExtensionsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ProjectExtensionsDataObject? extensions = null;
        try
        {
            var response = await _apiClient.GetBcf21ProjectExtensions(
                _projectId,
                _includeUsers,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve project extensions for project {ProjectId}. Status code: {StatusCode}", 
                    _projectId, response.StatusCode);
                throw new Exception($"Failed to retrieve project extensions. API StatusCode: {response.StatusCode}");
            }

            extensions = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving project extensions for project {ProjectId}", _projectId);
            throw;
        }

        if (extensions != null)
        {
            yield return extensions;
        }
    }
}
using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.Projects;

public class ProjectsDataReader : TypedAsyncDataReaderBase<ProjectsDataObject>
{
    private readonly ILogger<ProjectsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ProjectsDataReader(
        ILogger<ProjectsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ProjectsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<ProjectsDataObject>? projects = null;
        try
        {
            var response = await _apiClient.GetBcf21Projects(cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve projects. Status code: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve projects. API StatusCode: {response.StatusCode}");
            }

            projects = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving projects");
            throw;
        }

        if (projects != null)
        {
            foreach (var project in projects)
            {
                yield return project;
            }
        }
    }
}
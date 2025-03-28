using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF30.v1.Projects;

public class ProjectsDataReader : TypedAsyncDataReaderBase<ProjectsDataObject>
{
    private readonly ILogger<ProjectsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ProjectsDataReader(
        ILogger<ProjectsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ProjectsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<ProjectsDataObject>? projects = null;

        try
        {
            var response = await _apiClient.GetBcf30Projects(
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 projects. API StatusCode: {response.StatusCode}");
            }

            projects = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 projects");
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
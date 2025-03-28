using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using ESR.Hosting.CacheWriter;

namespace Connector.BCF21.v1.Versions;

public class VersionsDataReader : TypedAsyncDataReaderBase<VersionsDataObject>
{
    private readonly ILogger<VersionsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public VersionsDataReader(
        ILogger<VersionsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<VersionsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<IEnumerable<VersionsDataObject>>? response = null;

        try
        {
            response = await _apiClient.GetBcfVersions(cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF versions. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'VersionsDataObject'");
            throw;
        }

        if (response?.Data != null)
        {
            foreach (var version in response.Data)
            {
                yield return version;
            }
        }
    }
}
using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.CurrentUser;

public class CurrentUserDataReader : TypedAsyncDataReaderBase<CurrentUserDataObject>
{
    private readonly ILogger<CurrentUserDataReader> _logger;
    private readonly ApiClient _apiClient;

    public CurrentUserDataReader(
        ILogger<CurrentUserDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<CurrentUserDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<CurrentUserDataObject>? response = null;
        
        try
        {
            response = await _apiClient.GetBcf21CurrentUser(cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve current user. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'CurrentUserDataObject'");
            throw;
        }

        if (response?.Data != null)
        {
            yield return response.Data;
        }
    }
}
using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.UserClaims;

public class UserClaimsDataReader : TypedAsyncDataReaderBase<UserClaimsDataObject>
{
    private readonly ILogger<UserClaimsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public UserClaimsDataReader(
        ILogger<UserClaimsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<UserClaimsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<UserClaimsDataObject>? response = null;
        
        try
        {
            response = await _apiClient.GetBcf21UserClaims(cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve user claims. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'UserClaimsDataObject'");
            throw;
        }

        if (response?.Data != null)
        {
            yield return response.Data;
        }
    }
}
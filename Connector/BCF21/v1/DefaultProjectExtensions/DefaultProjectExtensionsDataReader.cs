using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using ESR.Hosting.CacheWriter;

namespace Connector.BCF21.v1.DefaultProjectExtensions;

internal static class DataObjectExtensions
{
    public static bool TryGetParameterValue<T>(this DataObjectCacheWriteArguments args, string key, out T? value)
    {
        value = default;
        if (args == null) return false;

        var dict = args.GetType().GetProperty("Arguments")?.GetValue(args) as IDictionary<string, object>;
        if (dict == null || !dict.ContainsKey(key)) return false;

        try
        {
            value = (T)dict[key];
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class DefaultProjectExtensionsDataReader : TypedAsyncDataReaderBase<DefaultProjectExtensionsDataObject>
{
    private readonly ILogger<DefaultProjectExtensionsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public DefaultProjectExtensionsDataReader(
        ILogger<DefaultProjectExtensionsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<DefaultProjectExtensionsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!dataObjectRunArguments.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
        {
            _logger.LogError("ProjectId is required for DefaultProjectExtensionsDataReader");
            yield break;
        }

        ApiResponse<DefaultProjectExtensionsDataObject> response;
        try
        {
            response = await _apiClient.GetBcf21DefaultProjectExtensions(
                projectId,
                cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving default project extensions");
            throw;
        }

        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Failed to retrieve default project extensions. Status code: {StatusCode}", response.StatusCode);
            yield break;
        }

        yield return response.Data;
    }
}
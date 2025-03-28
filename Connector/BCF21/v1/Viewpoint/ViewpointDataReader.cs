using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.BCF21.v1.Viewpoint;

internal static class DataObjectExtensions
{
    public static bool TryGetParameterValue<T>(this DataObjectCacheWriteArguments arguments, string key, out T? value)
    {
        value = default;
        if (arguments == null) return false;

        var dict = arguments.GetType().GetProperty("Parameters")?.GetValue(arguments) as IDictionary<string, object>;
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

public class ViewpointDataReader : TypedAsyncDataReaderBase<ViewpointDataObject>
{
    private readonly ILogger<ViewpointDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ViewpointDataReader(
        ILogger<ViewpointDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ViewpointDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<ViewpointDataObject>? response = null;
        string? projectId = null;
        string? topicId = null;
        string? viewpointId = null;
        
        try
        {
            if (!dataObjectRunArguments?.TryGetParameterValue("project_id", out projectId) ?? true || string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentException("Project ID is required");
            }

            if (!dataObjectRunArguments.TryGetParameterValue("topic_id", out topicId) || string.IsNullOrEmpty(topicId))
            {
                throw new ArgumentException("Topic ID is required");
            }

            if (!dataObjectRunArguments.TryGetParameterValue("viewpoint_id", out viewpointId) || string.IsNullOrEmpty(viewpointId))
            {
                throw new ArgumentException("Viewpoint ID is required");
            }

            response = await _apiClient.GetBcf21Viewpoint(
                projectId,
                topicId,
                viewpointId,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve viewpoint. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'ViewpointDataObject'");
            throw;
        }

        if (response?.Data != null)
        {
            yield return response.Data;
        }
    }
}
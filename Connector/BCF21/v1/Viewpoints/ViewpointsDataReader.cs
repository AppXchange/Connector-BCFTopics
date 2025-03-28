using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using ESR.Hosting.CacheWriter;

namespace Connector.BCF21.v1.Viewpoints;

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

public class ViewpointsDataReader : TypedAsyncDataReaderBase<ViewpointsDataObject>
{
    private readonly ILogger<ViewpointsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ViewpointsDataReader(
        ILogger<ViewpointsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ViewpointsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<IEnumerable<ViewpointsDataObject>>? response = null;
        string? projectId = null;
        string? topicId = null;
        
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

            response = await _apiClient.GetBcf21Viewpoints(
                projectId,
                topicId,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve viewpoints. API StatusCode: {response.StatusCode}");
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'ViewpointsDataObject'");
            throw;
        }

        if (response?.Data != null)
        {
            foreach (var viewpoint in response.Data)
            {
                yield return viewpoint;
            }
        }
    }
}
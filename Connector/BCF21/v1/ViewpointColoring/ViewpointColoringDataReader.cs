using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.ViewpointColoring;

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

public class ViewpointColoringDataReader : TypedAsyncDataReaderBase<ViewpointColoringDataObject>
{
    private readonly ILogger<ViewpointColoringDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ViewpointColoringDataReader(
        ILogger<ViewpointColoringDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ViewpointColoringDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            throw new ArgumentNullException(nameof(dataObjectRunArguments));
        }

        if (!dataObjectRunArguments.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
        {
            throw new ArgumentException("Project ID is required", nameof(dataObjectRunArguments));
        }

        if (!dataObjectRunArguments.TryGetParameterValue("topic_id", out string? topicId) || string.IsNullOrEmpty(topicId))
        {
            throw new ArgumentException("Topic ID is required", nameof(dataObjectRunArguments));
        }

        if (!dataObjectRunArguments.TryGetParameterValue("viewpoint_id", out string? viewpointId) || string.IsNullOrEmpty(viewpointId))
        {
            throw new ArgumentException("Viewpoint ID is required", nameof(dataObjectRunArguments));
        }

        ApiResponse<ViewpointColoringDataObject>? response = null;
        try
        {
            response = await _apiClient.GetBcf21ViewpointColoring(
                projectId,
                topicId,
                viewpointId,
                cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving viewpoint coloring for viewpoint {ViewpointId}", viewpointId);
            throw;
        }

        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to retrieve viewpoint coloring. Status code: {StatusCode}", response.StatusCode);
            yield break;
        }

        if (response.Data == null)
        {
            _logger.LogWarning("No coloring data received for viewpoint {ViewpointId}", viewpointId);
            yield break;
        }

        yield return response.Data;
    }
}
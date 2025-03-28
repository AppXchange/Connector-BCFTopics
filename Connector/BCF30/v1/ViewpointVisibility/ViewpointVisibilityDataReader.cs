using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF30.v1.ViewpointVisibility;

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

public class ViewpointVisibilityDataReader : TypedAsyncDataReaderBase<ViewpointVisibilityDataObject>
{
    private readonly ILogger<ViewpointVisibilityDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ViewpointVisibilityDataReader(
        ILogger<ViewpointVisibilityDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ViewpointVisibilityDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("DataObjectRunArguments is null");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
        {
            _logger.LogError("Project ID is null or empty");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("topic_id", out string? topicId) || string.IsNullOrEmpty(topicId))
        {
            _logger.LogError("Topic ID is null or empty");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("viewpoint_id", out string? viewpointId) || string.IsNullOrEmpty(viewpointId))
        {
            _logger.LogError("Viewpoint ID is null or empty");
            yield break;
        }

        ViewpointVisibilityDataObject? visibility = null;
        ApiResponse<ViewpointVisibilityDataObject>? response = null;

        try
        {
            response = await _apiClient.GetBcf30ViewpointVisibility(
                projectId,
                topicId,
                viewpointId,
                cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 viewpoint visibility. API StatusCode: {response.StatusCode}");
            }

            visibility = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 viewpoint visibility for project {ProjectId}, topic {TopicId}, and viewpoint {ViewpointId}", 
                projectId, topicId, viewpointId);
            throw;
        }

        if (visibility != null)
        {
            yield return visibility;
        }
    }
}
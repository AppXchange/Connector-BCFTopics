using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using ESR.Hosting.CacheWriter;

namespace Connector.BCF21.v1.RelatedTopics;

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

public class RelatedTopicsDataReader : TypedAsyncDataReaderBase<RelatedTopicsDataObject>
{
    private readonly ILogger<RelatedTopicsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public RelatedTopicsDataReader(
        ILogger<RelatedTopicsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<RelatedTopicsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("DataObjectRunArguments is required for RelatedTopicsDataReader");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
        {
            _logger.LogError("ProjectId is required for RelatedTopicsDataReader");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("topic_id", out string? topicId) || string.IsNullOrEmpty(topicId))
        {
            _logger.LogError("TopicId is required for RelatedTopicsDataReader");
            yield break;
        }

        ApiResponse<IEnumerable<RelatedTopicsDataObject>> response;
        try
        {
            response = await _apiClient.GetBcf21RelatedTopics(
                projectId,
                topicId,
                cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving related topics");
            throw;
        }

        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Failed to retrieve related topics. Status code: {StatusCode}", response.StatusCode);
            yield break;
        }

        foreach (var relatedTopic in response.Data)
        {
            yield return relatedTopic;
        }
    }
}
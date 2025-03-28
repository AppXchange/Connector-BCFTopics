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

namespace Connector.BCF21.v1.Topic;

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

public class TopicDataReader : TypedAsyncDataReaderBase<TopicDataObject>
{
    private readonly ILogger<TopicDataReader> _logger;
    private readonly ApiClient _apiClient;

    public TopicDataReader(
        ILogger<TopicDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<TopicDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("DataObjectRunArguments is required for TopicDataReader");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
        {
            _logger.LogError("ProjectId is required for TopicDataReader");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("topic_id", out string? topicId) || string.IsNullOrEmpty(topicId))
        {
            _logger.LogError("TopicId is required for TopicDataReader");
            yield break;
        }

        ApiResponse<TopicDataObject> response;
        try
        {
            response = await _apiClient.GetBcf21Topic(
                projectId,
                topicId,
                cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving topic");
            throw;
        }

        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Failed to retrieve topic. Status code: {StatusCode}", response.StatusCode);
            yield break;
        }

        yield return response.Data;
    }
}
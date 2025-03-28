using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF30.v1.Topics;

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

public class TopicsDataReader : TypedAsyncDataReaderBase<TopicsDataObject>
{
    private readonly ILogger<TopicsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public TopicsDataReader(
        ILogger<TopicsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<TopicsDataObject> GetTypedDataAsync(
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

        IEnumerable<TopicsDataObject>? topics = null;

        try
        {
            var response = await _apiClient.GetBcf30Topics(
                projectId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve BCF 3.0 topics. API StatusCode: {response.StatusCode}");
            }

            topics = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, 
                "Exception while retrieving BCF 3.0 topics for project {ProjectId}", 
                projectId);
            throw;
        }

        if (topics != null)
        {
            foreach (var topic in topics)
            {
                yield return topic;
            }
        }
    }
}
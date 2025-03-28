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

namespace Connector.BCF21.v1.TopicsSyncingObjects;

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

public class TopicsSyncingObjectsDataReader : TypedAsyncDataReaderBase<TopicsSyncingObjectsDataObject>
{
    private readonly ILogger<TopicsSyncingObjectsDataReader> _logger;
    private readonly ApiClient _apiClient;
    private string? _skipToken;
    private bool _hasMorePages = true;

    public TopicsSyncingObjectsDataReader(
        ILogger<TopicsSyncingObjectsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<TopicsSyncingObjectsDataObject> GetTypedDataAsync(
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

        if (!dataObjectRunArguments.TryGetParameterValue("type", out string? type) || string.IsNullOrEmpty(type))
        {
            throw new ArgumentException("Type is required", nameof(dataObjectRunArguments));
        }

        dataObjectRunArguments.TryGetParameterValue("fetchAll", out bool fetchAll);
        dataObjectRunArguments.TryGetParameterValue("pageSize", out int pageSize);

        while (_hasMorePages)
        {
            var response = await _apiClient.GetBcf21SyncingObjects(
                projectId,
                type,
                fetchAll,
                pageSize,
                _skipToken,
                cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve syncing objects. API StatusCode: {response.StatusCode}");
            }

            if (response.Data == null)
            {
                break;
            }

            yield return response.Data;

            _skipToken = response.Data.Links.Next.Href;
            _hasMorePages = !string.IsNullOrEmpty(_skipToken);
        }
    }
}
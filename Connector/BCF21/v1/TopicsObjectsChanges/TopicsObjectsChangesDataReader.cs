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

namespace Connector.BCF21.v1.TopicsObjectsChanges;

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

public class TopicsObjectsChangesDataReader : TypedAsyncDataReaderBase<TopicsObjectsChangesDataObject>
{
    private readonly ILogger<TopicsObjectsChangesDataReader> _logger;
    private readonly ApiClient _apiClient;
    private string? _skipToken;
    private bool _hasMorePages = true;

    public TopicsObjectsChangesDataReader(
        ILogger<TopicsObjectsChangesDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<TopicsObjectsChangesDataObject> GetTypedDataAsync(
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

        if (!dataObjectRunArguments.TryGetParameterValue("changeToken", out string? changeToken))
        {
            throw new ArgumentException("Change Token is required", nameof(dataObjectRunArguments));
        }

        if (string.IsNullOrEmpty(changeToken))
        {
            throw new ArgumentException("Change Token cannot be null or empty", nameof(dataObjectRunArguments));
        }

        dataObjectRunArguments.TryGetParameterValue("pageSize", out int pageSize);

        while (_hasMorePages)
        {
            TopicsObjectsChangesDataObject? responseData = null;
            try
            {
                var response = await _apiClient.GetBcf21TopicsObjectsChanges(
                    projectId,
                    type,
                    changeToken,
                    pageSize,
                    _skipToken,
                    cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Failed to retrieve changes. API StatusCode: {response.StatusCode}");
                }

                responseData = response.Data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving changes");
                throw;
            }

            if (responseData == null)
            {
                break;
            }

            yield return responseData;

            _skipToken = responseData.Links.Next.Href;
            _hasMorePages = !string.IsNullOrEmpty(_skipToken);
        }
    }
}
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
using System.Web;

namespace Connector.BCF21.v1.Topics;

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
    private string? _skipToken;

    public TopicsDataReader(
        ILogger<TopicsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<TopicsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("DataObjectRunArguments is required for TopicsDataReader");
            yield break;
        }

        if (!dataObjectRunArguments.TryGetParameterValue("project_id", out string? projectId) || string.IsNullOrEmpty(projectId))
        {
            _logger.LogError("ProjectId is required for TopicsDataReader");
            yield break;
        }

        while (true)
        {
            ApiResponse<IEnumerable<TopicsDataObject>> response;
            try
            {
                response = await _apiClient.GetBcf21Topics(
                    projectId,
                    top: 500,
                    skipToken: _skipToken,
                    cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving topics");
                throw;
            }

            if (!response.IsSuccessful || response.Data == null)
            {
                _logger.LogError("Failed to retrieve topics. Status code: {StatusCode}", response.StatusCode);
                yield break;
            }

            foreach (var topic in response.Data)
            {
                yield return topic;
            }

            // Check if there are more pages
            if (response.Headers.TryGetValues("next", out var nextValues))
            {
                var nextUrl = nextValues.FirstOrDefault();
                if (string.IsNullOrEmpty(nextUrl))
                {
                    break;
                }
                _skipToken = ExtractSkipToken(nextUrl);
            }
            else
            {
                break;
            }
        }
    }

    private string? ExtractSkipToken(string nextUrl)
    {
        try
        {
            var uri = new Uri(nextUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query["skiptoken"];
        }
        catch
        {
            return null;
        }
    }
}
using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.BCF21.v1.RelatedTopics;

/// <summary>
/// Represents a related topic in BCF 2.1
/// </summary>
[Description("Represents a related topic in BCF 2.1")]
[PrimaryKey("related_topic_guid")]
public class RelatedTopicsDataObject
{
    [JsonPropertyName("related_topic_guid")]
    [Description("The GUID of the related topic")]
    [Required]
    public string RelatedTopicGuid { get; init; } = string.Empty;
}
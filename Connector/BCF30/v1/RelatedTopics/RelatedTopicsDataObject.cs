namespace Connector.BCF30.v1.RelatedTopics;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a related topic in BCF 3.0
/// </summary>
[PrimaryKey("related_topic_guid", nameof(RelatedTopicGuid))]
[Description("BCF 3.0 Related Topic object representing a reference to another topic")]
public class RelatedTopicsDataObject
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project this related topic belongs to")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The ID of the topic this related topic belongs to")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("related_topic_guid")]
    [Description("The globally unique identifier of the related topic")]
    [Required]
    public required string RelatedTopicGuid { get; init; }
}
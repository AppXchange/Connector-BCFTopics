namespace Connector.BCF30.v1.Topics;

using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a topic in BCF 3.0
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("BCF 3.0 Topic object representing an issue or discussion topic")]
public class TopicsDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the topic")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("server_assigned_id")]
    [Description("The server-assigned identifier of the topic")]
    [Required]
    public required string ServerAssignedId { get; init; }

    [JsonPropertyName("creation_author")]
    [Description("The author who created the topic")]
    [Required]
    public required string CreationAuthor { get; init; }

    [JsonPropertyName("title")]
    [Description("The title of the topic")]
    [Required]
    public required string Title { get; init; }

    [JsonPropertyName("labels")]
    [Description("The labels associated with the topic")]
    [Required]
    public required IEnumerable<string> Labels { get; init; }

    [JsonPropertyName("creation_date")]
    [Description("The date and time when the topic was created")]
    [Required]
    public required DateTime CreationDate { get; init; }

    [JsonPropertyName("authorization")]
    [Description("The authorization information for the topic")]
    public TopicAuthorization? Authorization { get; init; }
}

public class TopicAuthorization
{
    [JsonPropertyName("topic_actions")]
    [Description("List of actions the current user is authorized to perform on the topic")]
    [Required]
    public required IEnumerable<string> TopicActions { get; init; }
}
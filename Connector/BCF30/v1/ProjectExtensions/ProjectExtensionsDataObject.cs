namespace Connector.BCF30.v1.ProjectExtensions;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing project extensions in BCF 3.0
/// </summary>
[PrimaryKey("project_id", nameof(ProjectId))]
[Description("BCF 3.0 Project Extensions object representing possible values that can be used in topics and comments")]
public class ProjectExtensionsDataObject
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project these extensions belong to")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_type")]
    [Description("List of available topic types")]
    [Required]
    public required IEnumerable<string> TopicType { get; init; }

    [JsonPropertyName("topic_status")]
    [Description("List of available topic statuses")]
    [Required]
    public required IEnumerable<string> TopicStatus { get; init; }

    [JsonPropertyName("topic_label")]
    [Description("List of available topic labels")]
    [Required]
    public required IEnumerable<string> TopicLabel { get; init; }

    [JsonPropertyName("snippet_type")]
    [Description("List of available snippet types")]
    [Required]
    public required IEnumerable<string> SnippetType { get; init; }

    [JsonPropertyName("priority")]
    [Description("List of available priority levels")]
    [Required]
    public required IEnumerable<string> Priority { get; init; }

    [JsonPropertyName("users")]
    [Description("List of available users")]
    [Required]
    public required IEnumerable<string> Users { get; init; }

    [JsonPropertyName("stage")]
    [Description("List of available project stages")]
    [Required]
    public required IEnumerable<string> Stage { get; init; }

    [JsonPropertyName("project_actions")]
    [Description("List of available project actions")]
    [Required]
    public required IEnumerable<string> ProjectActions { get; init; }

    [JsonPropertyName("topic_actions")]
    [Description("List of available topic actions")]
    [Required]
    public required IEnumerable<string> TopicActions { get; init; }

    [JsonPropertyName("comment_actions")]
    [Description("List of available comment actions")]
    [Required]
    public required IEnumerable<string> CommentActions { get; init; }
}
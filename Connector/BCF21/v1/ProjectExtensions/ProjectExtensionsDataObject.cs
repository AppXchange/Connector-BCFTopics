namespace Connector.BCF21.v1.ProjectExtensions;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Represents the project extensions in BCF 2.1
/// </summary>
[PrimaryKey("version", nameof(Version))]
[Description("Represents the project extensions in BCF 2.1")]
public class ProjectExtensionsDataObject
{
    [JsonPropertyName("version")]
    [Description("The version of the project extensions")]
    public int Version { get; init; }

    [JsonPropertyName("snippet_type")]
    [Description("Available snippet types")]
    public List<string> SnippetType { get; init; } = new();

    [JsonPropertyName("user_id_type")]
    [Description("Available user ID types")]
    public List<string> UserIdType { get; init; } = new();

    [JsonPropertyName("project_actions")]
    [Description("Available project actions")]
    public List<string> ProjectActions { get; init; } = new();

    [JsonPropertyName("topic_actions")]
    [Description("Available topic actions")]
    public List<string> TopicActions { get; init; } = new();

    [JsonPropertyName("comment_actions")]
    [Description("Available comment actions")]
    public List<string> CommentActions { get; init; } = new();

    [JsonPropertyName("topic_type_full")]
    [Description("Available topic types with full details")]
    public List<ExtensionItem> TopicTypeFull { get; init; } = new();

    [JsonPropertyName("topic_status_full")]
    [Description("Available topic statuses with full details")]
    public List<ExtensionItem> TopicStatusFull { get; init; } = new();

    [JsonPropertyName("topic_label_full")]
    [Description("Available topic labels with full details")]
    public List<ExtensionItem> TopicLabelFull { get; init; } = new();

    [JsonPropertyName("priority_full")]
    [Description("Available priorities with full details")]
    public List<ExtensionItem> PriorityFull { get; init; } = new();

    [JsonPropertyName("stage_full")]
    [Description("Available stages with full details")]
    public List<ExtensionItem> StageFull { get; init; } = new();

    [JsonPropertyName("topic_type")]
    [Description("Available topic types")]
    public List<string> TopicType { get; init; } = new();

    [JsonPropertyName("topic_status")]
    [Description("Available topic statuses")]
    public List<string> TopicStatus { get; init; } = new();

    [JsonPropertyName("topic_label")]
    [Description("Available topic labels")]
    public List<string> TopicLabel { get; init; } = new();

    [JsonPropertyName("priority")]
    [Description("Available priorities")]
    public List<string> Priority { get; init; } = new();

    [JsonPropertyName("stage")]
    [Description("Available stages")]
    public List<string> Stage { get; init; } = new();
}

public class ExtensionItem
{
    [JsonPropertyName("name")]
    [Description("The name of the extension item")]
    [Required]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    [Description("The color associated with the extension item")]
    public string? Color { get; init; }

    [JsonPropertyName("icon")]
    [Description("The icon associated with the extension item")]
    public string? Icon { get; init; }

    [JsonPropertyName("visible")]
    [Description("Whether the extension item is visible")]
    public bool Visible { get; init; }

    [JsonPropertyName("isdefault")]
    [Description("Whether the extension item is the default")]
    public bool IsDefault { get; init; }
}
namespace Connector.BCF21.v1.DefaultProjectExtensions;

using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Default extension values for a BCF 2.1 project")]
public class DefaultProjectExtensionsDataObject
{
    [JsonPropertyName("id")]
    [Description("Example primary key of the object")]
    [Required]
    public required Guid Id { get; init; }

    [JsonPropertyName("version")]
    [Description("The version of the default extensions")]
    public int Version { get; set; }

    [JsonPropertyName("snippet_type")]
    [Description("Available snippet types")]
    public List<string> SnippetType { get; set; } = new();

    [JsonPropertyName("user_id_type")]
    [Description("Available user ID types")]
    public List<string> UserIdType { get; set; } = new();

    [JsonPropertyName("project_actions")]
    [Description("Available project actions")]
    public List<string> ProjectActions { get; set; } = new();

    [JsonPropertyName("topic_actions")]
    [Description("Available topic actions")]
    public List<string> TopicActions { get; set; } = new();

    [JsonPropertyName("comment_actions")]
    [Description("Available comment actions")]
    public List<string> CommentActions { get; set; } = new();

    [JsonPropertyName("topic_type_full")]
    [Description("Available topic types with full details")]
    public List<ExtensionItem> TopicTypeFull { get; set; } = new();

    [JsonPropertyName("topic_status_full")]
    [Description("Available topic statuses with full details")]
    public List<ExtensionItem> TopicStatusFull { get; set; } = new();

    [JsonPropertyName("topic_label_full")]
    [Description("Available topic labels with full details")]
    public List<ExtensionItem> TopicLabelFull { get; set; } = new();

    [JsonPropertyName("priority_full")]
    [Description("Available priorities with full details")]
    public List<ExtensionItem> PriorityFull { get; set; } = new();

    [JsonPropertyName("stage_full")]
    [Description("Available stages with full details")]
    public List<ExtensionItem> StageFull { get; set; } = new();

    [JsonPropertyName("topic_type")]
    [Description("Available topic types")]
    public List<string> TopicType { get; set; } = new();

    [JsonPropertyName("topic_status")]
    [Description("Available topic statuses")]
    public List<string> TopicStatus { get; set; } = new();

    [JsonPropertyName("topic_label")]
    [Description("Available topic labels")]
    public List<string> TopicLabel { get; set; } = new();

    [JsonPropertyName("priority")]
    [Description("Available priorities")]
    public List<string> Priority { get; set; } = new();

    [JsonPropertyName("stage")]
    [Description("Available stages")]
    public List<string> Stage { get; set; } = new();
}

public class ExtensionItem
{
    [JsonPropertyName("name")]
    [Description("The name of the extension item")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    [Description("The color associated with the extension item")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    [Description("The icon associated with the extension item")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("visible")]
    [Description("Whether the extension item is visible")]
    public bool Visible { get; set; }

    [JsonPropertyName("isdefault")]
    [Description("Whether the extension item is the default")]
    public bool IsDefault { get; set; }
}
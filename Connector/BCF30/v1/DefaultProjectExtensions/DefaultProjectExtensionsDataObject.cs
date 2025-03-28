namespace Connector.BCF30.v1.DefaultProjectExtensions;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;
using System.Collections.Generic;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("project_id", nameof(ProjectId))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("BCF 3.0 Default Project Extensions object representing default values that can be used in topics")]
public class DefaultProjectExtensionsDataObject
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project these default extensions belong to")]
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

    [JsonPropertyName("topic_type_full")]
    [Description("List of topic type definitions with their display properties")]
    [Required]
    public required IEnumerable<ExtensionValue> TopicTypeFull { get; init; }

    [JsonPropertyName("topic_status_full")]
    [Description("List of topic status definitions with their display properties")]
    [Required]
    public required IEnumerable<ExtensionValue> TopicStatusFull { get; init; }

    [JsonPropertyName("topic_label_full")]
    [Description("List of topic label definitions with their display properties")]
    [Required]
    public required IEnumerable<ExtensionValue> TopicLabelFull { get; init; }

    [JsonPropertyName("priority_full")]
    [Description("List of priority definitions with their display properties")]
    [Required]
    public required IEnumerable<ExtensionValue> PriorityFull { get; init; }
}

public class ExtensionValue
{
    [JsonPropertyName("name")]
    [Description("The name of the extension value")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("color")]
    [Description("The color associated with the extension value")]
    [Required]
    public required string Color { get; init; }

    [JsonPropertyName("icon")]
    [Description("The icon associated with the extension value")]
    public string? Icon { get; init; }

    [JsonPropertyName("visible")]
    [Description("Whether the extension value is visible")]
    [Required]
    public required bool Visible { get; init; }

    [JsonPropertyName("isdefault")]
    [Description("Whether this is the default value")]
    [Required]
    public required bool IsDefault { get; init; }
}
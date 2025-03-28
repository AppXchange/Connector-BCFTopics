namespace Connector.BCF30.v1.ProjectExtensions.Update;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for updating project extensions in BCF 3.0
/// </summary>
[Description("Updates the project extensions in BCF 3.0, defining possible values that can be used in topics")]
public class UpdateProjectExtensionsAction : IStandardAction<UpdateProjectExtensionsActionInput, UpdateProjectExtensionsActionOutput>
{
    public UpdateProjectExtensionsActionInput ActionInput { get; set; } = null!;
    public UpdateProjectExtensionsActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
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

public class UpdateProjectExtensionsActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project to update extensions for")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("priority_full")]
    [Description("List of priority definitions with their display properties")]
    [Required]
    public required IEnumerable<ExtensionValue> PriorityFull { get; init; }

    [JsonPropertyName("topic_server_assigned_id_prefix")]
    [Description("The prefix for server-assigned topic IDs")]
    [Required]
    public required string TopicServerAssignedIdPrefix { get; init; }
}

public class UpdateProjectExtensionsActionOutput
{
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

    [JsonPropertyName("topic_server_assigned_id_prefix")]
    [Description("The prefix for server-assigned topic IDs")]
    [Required]
    public required string TopicServerAssignedIdPrefix { get; init; }

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

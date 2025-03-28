namespace Connector.BCF21.v1.ProjectExtensions.Update;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Updates the project extensions in BCF 2.1
/// </summary>
[Description("Updates the project extensions in BCF 2.1")]
public class UpdateProjectExtensionsAction : IStandardAction<UpdateProjectExtensionsActionInput, UpdateProjectExtensionsActionOutput>
{
    public UpdateProjectExtensionsActionInput ActionInput { get; set; } = new();
    public UpdateProjectExtensionsActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateProjectExtensionsActionInput
{
    [JsonPropertyName("projectId")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("snippet_type")]
    [Description("Available snippet types")]
    public List<string>? SnippetType { get; set; }

    [JsonPropertyName("topic_type_full")]
    [Description("Available topic types with full details")]
    public List<ExtensionItem>? TopicTypeFull { get; set; }

    [JsonPropertyName("topic_status_full")]
    [Description("Available topic statuses with full details")]
    public List<ExtensionItem>? TopicStatusFull { get; set; }

    [JsonPropertyName("topic_label_full")]
    [Description("Available topic labels with full details")]
    public List<ExtensionItem>? TopicLabelFull { get; set; }

    [JsonPropertyName("priority_full")]
    [Description("Available priorities with full details")]
    public List<ExtensionItem>? PriorityFull { get; set; }

    [JsonPropertyName("stage_full")]
    [Description("Available stages with full details")]
    public List<ExtensionItem>? StageFull { get; set; }

    [JsonPropertyName("topic_type")]
    [Description("Available topic types")]
    public List<string>? TopicType { get; set; }

    [JsonPropertyName("topic_status")]
    [Description("Available topic statuses")]
    public List<string>? TopicStatus { get; set; }

    [JsonPropertyName("topic_label")]
    [Description("Available topic labels")]
    public List<string>? TopicLabel { get; set; }

    [JsonPropertyName("priority")]
    [Description("Available priorities")]
    public List<string>? Priority { get; set; }

    [JsonPropertyName("stage")]
    [Description("Available stages")]
    public List<string>? Stage { get; set; }
}

public class UpdateProjectExtensionsActionOutput
{
    [JsonPropertyName("version")]
    [Description("The version of the project extensions")]
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

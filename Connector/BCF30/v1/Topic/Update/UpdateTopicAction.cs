namespace Connector.BCF30.v1.Topic.Update;

using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for updating a topic in BCF 3.0
/// </summary>
[Description("Updates an existing topic in BCF 3.0")]
public class UpdateTopicAction : IStandardAction<UpdateTopicActionInput, UpdateTopicActionOutput>
{
    public UpdateTopicActionInput ActionInput { get; set; } = null!;
    public UpdateTopicActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateTopicActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project containing the topic")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The ID of the topic to update")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("topic_type")]
    [Description("The type of the topic")]
    [Required]
    public required string TopicType { get; init; }

    [JsonPropertyName("topic_status")]
    [Description("The status of the topic")]
    [Required]
    public required string TopicStatus { get; init; }

    [JsonPropertyName("title")]
    [Description("The title of the topic")]
    [Required]
    public required string Title { get; init; }

    [JsonPropertyName("priority")]
    [Description("The priority level of the topic")]
    [Required]
    public required string Priority { get; init; }

    [JsonPropertyName("labels")]
    [Description("The labels associated with the topic")]
    [Required]
    public required IEnumerable<string> Labels { get; init; }

    [JsonPropertyName("assigned_to")]
    [Description("The user assigned to the topic")]
    public string? AssignedTo { get; init; }

    [JsonPropertyName("bim_snippet")]
    [Description("BIM snippet associated with the topic")]
    public BimSnippetInput? BimSnippet { get; init; }
}

public class BimSnippetInput
{
    [JsonPropertyName("snippet_type")]
    [Description("The type of the BIM snippet")]
    [Required]
    public required string SnippetType { get; init; }

    [JsonPropertyName("is_external")]
    [Description("Whether the snippet is externally hosted")]
    [Required]
    public required bool IsExternal { get; init; }

    [JsonPropertyName("reference")]
    [Description("Reference to the BIM snippet")]
    [Required]
    public required string Reference { get; init; }

    [JsonPropertyName("reference_schema")]
    [Description("Schema for the BIM snippet reference")]
    [Required]
    public required string ReferenceSchema { get; init; }
}

public class UpdateTopicActionOutput
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

    [JsonPropertyName("creation_date")]
    [Description("The date and time when the topic was created")]
    [Required]
    public required DateTime CreationDate { get; init; }

    [JsonPropertyName("modified_author")]
    [Description("The author who last modified the topic")]
    [Required]
    public required string ModifiedAuthor { get; init; }

    [JsonPropertyName("modified_date")]
    [Description("The date and time when the topic was last modified")]
    [Required]
    public required DateTime ModifiedDate { get; init; }

    [JsonPropertyName("topic_type")]
    [Description("The type of the topic")]
    [Required]
    public required string TopicType { get; init; }

    [JsonPropertyName("topic_status")]
    [Description("The status of the topic")]
    [Required]
    public required string TopicStatus { get; init; }

    [JsonPropertyName("title")]
    [Description("The title of the topic")]
    [Required]
    public required string Title { get; init; }

    [JsonPropertyName("priority")]
    [Description("The priority level of the topic")]
    [Required]
    public required string Priority { get; init; }

    [JsonPropertyName("labels")]
    [Description("The labels associated with the topic")]
    [Required]
    public required IEnumerable<string> Labels { get; init; }

    [JsonPropertyName("assigned_to")]
    [Description("The user assigned to the topic")]
    public string? AssignedTo { get; init; }

    [JsonPropertyName("bim_snippet")]
    [Description("BIM snippet associated with the topic")]
    public BimSnippetInput? BimSnippet { get; init; }
}

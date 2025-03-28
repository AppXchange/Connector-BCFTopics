namespace Connector.BCF21.v1.TopicsBatch.Update;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object that will represent an action in the Xchange system. This will contain an input object type,
/// an output object type, and a Action failure type (this will default to <see cref="StandardActionFailure"/>
/// but that can be overridden with your own preferred type). These objects will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[Description("Updates Topics in batch for the specified Trimble Connect Project")]
public class UpdateTopicsBatchAction : IStandardAction<UpdateTopicsBatchActionInput, UpdateTopicsBatchActionOutput>
{
    public UpdateTopicsBatchActionInput ActionInput { get; set; } = new() { Items = Array.Empty<UpdateTopicBatchItem>() };
    public UpdateTopicsBatchActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateTopicsBatchActionInput
{
    [JsonPropertyName("items")]
    [Description("Collection of topics to update")]
    [Required]
    public required UpdateTopicBatchItem[] Items { get; set; }
}

public class UpdateTopicBatchItem
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the topic")]
    [Required]
    public required string Guid { get; set; }

    [JsonPropertyName("title")]
    [Description("The title of the topic")]
    [Required]
    public required string Title { get; set; }

    [JsonPropertyName("topic_type")]
    [Description("The type of the topic")]
    [Required]
    public required string TopicType { get; set; }

    [JsonPropertyName("description")]
    [Description("The description of the topic")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("assigned_to")]
    [Description("The user assigned to the topic")]
    public string? AssignedTo { get; set; }

    [JsonPropertyName("assigned_to_uuid")]
    [Description("The UUID of the user assigned to the topic")]
    public string? AssignedToUuid { get; set; }

    [JsonPropertyName("topic_status")]
    [Description("The status of the topic")]
    [Required]
    public required string TopicStatus { get; set; }

    [JsonPropertyName("reference_links")]
    [Description("Collection of reference links")]
    public string[] ReferenceLinks { get; set; } = Array.Empty<string>();

    [JsonPropertyName("labels")]
    [Description("Collection of labels")]
    public string[] Labels { get; set; } = Array.Empty<string>();

    [JsonPropertyName("priority")]
    [Description("The priority level of the topic")]
    public string Priority { get; set; } = string.Empty;

    [JsonPropertyName("index")]
    [Description("The index of the topic")]
    public int Index { get; set; }

    [JsonPropertyName("stage")]
    [Description("The stage of the topic")]
    public string Stage { get; set; } = string.Empty;

    [JsonPropertyName("due_date")]
    [Description("The due date of the topic")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("assignees")]
    [Description("Collection of assignees")]
    public Assignee[] Assignees { get; set; } = Array.Empty<Assignee>();

    [JsonPropertyName("marker")]
    [Description("The marker information")]
    public Marker? Marker { get; set; }

    [JsonPropertyName("bim_snippet")]
    [Description("The BIM snippet information")]
    public BimSnippet? BimSnippet { get; set; }
}

public class UpdateTopicsBatchActionOutput
{
    [JsonPropertyName("items")]
    [Description("Collection of updated topics")]
    public TopicItem[] Items { get; set; } = Array.Empty<TopicItem>();

    [JsonPropertyName("errors")]
    [Description("Collection of errors that occurred during batch processing")]
    public BatchError[] Errors { get; set; } = Array.Empty<BatchError>();
}

public class Assignee
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

public class Marker
{
    [JsonPropertyName("model_point")]
    public ModelPoint ModelPoint { get; set; } = new();
}

public class ModelPoint
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }
}

public class BimSnippet
{
    [JsonPropertyName("snippet_type")]
    public string SnippetType { get; set; } = string.Empty;

    [JsonPropertyName("is_external")]
    public bool IsExternal { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("reference_schema")]
    public string ReferenceSchema { get; set; } = string.Empty;
}

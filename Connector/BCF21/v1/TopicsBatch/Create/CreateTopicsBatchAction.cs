namespace Connector.BCF21.v1.TopicsBatch.Create;

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
[Description("Creates Topics in batch for the specified Trimble Connect Project")]
public class CreateTopicsBatchAction : IStandardAction<CreateTopicsBatchActionInput, CreateTopicsBatchActionOutput>
{
    public CreateTopicsBatchActionInput ActionInput { get; set; } = new() { Items = Array.Empty<TopicBatchItem>() };
    public CreateTopicsBatchActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateTopicsBatchActionInput
{
    [JsonPropertyName("items")]
    [Description("Collection of topics to create")]
    [Required]
    public required TopicBatchItem[] Items { get; set; }

    [JsonPropertyName("validation")]
    [Description("Validation behavior for project extensions")]
    public string? Validation { get; set; }
}

public class TopicBatchItem
{
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
}

public class CreateTopicsBatchActionOutput
{
    [JsonPropertyName("items")]
    [Description("Collection of created topics")]
    public TopicItem[] Items { get; set; } = Array.Empty<TopicItem>();

    [JsonPropertyName("errors")]
    [Description("Collection of errors that occurred during batch processing")]
    public BatchError[] Errors { get; set; } = Array.Empty<BatchError>();
}

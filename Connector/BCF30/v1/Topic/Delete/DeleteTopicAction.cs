namespace Connector.BCF30.v1.Topic.Delete;

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
[Description("DeleteTopicAction Action description goes here")]
public class DeleteTopicAction : IStandardAction<DeleteTopicActionInput, DeleteTopicActionOutput>
{
    public DeleteTopicActionInput ActionInput { get; set; } = null!;
    public DeleteTopicActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;

    public string ActionType => "BCF30DeleteTopic";
}

public class DeleteTopicActionInput
{
    [JsonPropertyName("project_id")]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    public required string TopicId { get; init; }
}

public class DeleteTopicActionOutput
{
    // Empty output class as delete operation doesn't return any data
}

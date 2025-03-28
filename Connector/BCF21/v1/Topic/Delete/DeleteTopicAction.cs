namespace Connector.BCF21.v1.Topic.Delete;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object that represents the deletion of a Topic in a BCF 2.1 project
/// </summary>
[Description("Deletes an existing Topic from a BCF 2.1 project")]
public class DeleteTopicAction : IStandardAction<DeleteTopicActionInput, DeleteTopicActionOutput>
{
    public DeleteTopicActionInput ActionInput { get; set; } = new();
    public DeleteTopicActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

/// <summary>
/// Input model for deleting a topic
/// </summary>
public class DeleteTopicActionInput
{
    // No input parameters needed as project_id and topic_id are passed as parameters
}

/// <summary>
/// Output model for topic deletion
/// </summary>
public class DeleteTopicActionOutput
{
    [JsonPropertyName("success")]
    [Description("Indicates whether the deletion was successful")]
    public bool Success { get; set; }
}

namespace Connector.BCF21.v1.Comment.Delete;

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
[Description("Deletes a comment from a specified topic in BCF 2.1")]
public class DeleteCommentAction : IStandardAction<DeleteCommentActionInput, DeleteCommentActionOutput>
{
    public DeleteCommentActionInput ActionInput { get; set; } = new();
    public DeleteCommentActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class DeleteCommentActionInput
{
    [JsonPropertyName("projectId")]
    [Description("The id of the Trimble Connect Project")]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("topicId")]
    [Description("The id of the Topic")]
    public string TopicId { get; set; } = string.Empty;

    [JsonPropertyName("commentId")]
    [Description("The id of the Comment to be deleted")]
    public string CommentId { get; set; } = string.Empty;
}

public class DeleteCommentActionOutput
{
    // No output properties needed for delete operation as it returns 204 No Content
}

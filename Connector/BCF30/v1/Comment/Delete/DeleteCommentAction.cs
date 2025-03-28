namespace Connector.BCF30.v1.Comment.Delete;

using Json.Schema.Generation;
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
[Description("Deletes a BCF 3.0 comment from a topic")]
public class DeleteCommentAction : IStandardAction<DeleteCommentActionInput, DeleteCommentActionOutput>
{
    public DeleteCommentActionInput ActionInput { get; set; } = null!;
    public DeleteCommentActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class DeleteCommentActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The id of the topic containing the comment")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("comment_id")]
    [Description("The id of the comment to delete")]
    [Required]
    public required string CommentId { get; init; }
}

public class DeleteCommentActionOutput
{
    // No output for delete operation as it returns 204 No Content
}

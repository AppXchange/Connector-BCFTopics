namespace Connector.BCF21.v1.CommentsBatch.Update;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Updates Comments in batch for the specified Trimble Connect Project
/// </summary>
[Description("Updates Comments in batch for the specified Trimble Connect Project")]
public class UpdateCommentsBatchAction : IStandardAction<UpdateCommentsBatchActionInput, UpdateCommentsBatchActionOutput>
{
    public UpdateCommentsBatchActionInput ActionInput { get; set; } = new() { Items = Array.Empty<CommentBatchUpdateItem>() };
    public UpdateCommentsBatchActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateCommentsBatchActionInput
{
    [JsonPropertyName("items")]
    [Description("Collection of comments to update")]
    [Required]
    public required CommentBatchUpdateItem[] Items { get; set; }
}

public class CommentBatchUpdateItem
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the comment")]
    [Required]
    public required string Guid { get; set; }

    [JsonPropertyName("comment")]
    [Description("The comment text")]
    [Required]
    public required string Comment { get; set; }

    [JsonPropertyName("topic_guid")]
    [Description("The globally unique identifier of the topic")]
    [Required]
    public required string TopicGuid { get; set; }

    [JsonPropertyName("viewpoint_guid")]
    [Description("The globally unique identifier of the viewpoint")]
    public string? ViewpointGuid { get; set; }

    [JsonPropertyName("reply_to_comment_guid")]
    [Description("The globally unique identifier of the comment being replied to")]
    public string? ReplyToCommentGuid { get; set; }
}

public class UpdateCommentsBatchActionOutput
{
    [JsonPropertyName("items")]
    [Description("Collection of updated comments")]
    public CommentItem[] Items { get; set; } = Array.Empty<CommentItem>();

    [JsonPropertyName("errors")]
    [Description("Collection of errors that occurred during batch processing")]
    public BatchError[] Errors { get; set; } = Array.Empty<BatchError>();
}

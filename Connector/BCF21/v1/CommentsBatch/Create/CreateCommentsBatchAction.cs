namespace Connector.BCF21.v1.CommentsBatch.Create;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Creates Comments in batch for the specified Trimble Connect Project
/// </summary>
[Description("Creates Comments in batch for the specified Trimble Connect Project")]
public class CreateCommentsBatchAction : IStandardAction<CreateCommentsBatchActionInput, CreateCommentsBatchActionOutput>
{
    public CreateCommentsBatchActionInput ActionInput { get; set; } = new() { Items = Array.Empty<CommentBatchItem>() };
    public CreateCommentsBatchActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateCommentsBatchActionInput
{
    [JsonPropertyName("items")]
    [Description("Collection of comments to create")]
    [Required]
    public required CommentBatchItem[] Items { get; set; }
}

public class CommentBatchItem
{
    [JsonPropertyName("comment")]
    [Description("The comment text")]
    [Required]
    public required string Comment { get; set; }

    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the comment")]
    public string? Guid { get; set; }

    [JsonPropertyName("date")]
    [Description("The date of the comment")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("author")]
    [Description("The author of the comment")]
    public string? Author { get; set; }

    [JsonPropertyName("author_uuid")]
    [Description("The UUID of the author")]
    public string? AuthorUuid { get; set; }

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

public class CreateCommentsBatchActionOutput
{
    [JsonPropertyName("items")]
    [Description("Collection of created comments")]
    public CommentItem[] Items { get; set; } = Array.Empty<CommentItem>();

    [JsonPropertyName("errors")]
    [Description("Collection of errors that occurred during batch processing")]
    public BatchError[] Errors { get; set; } = Array.Empty<BatchError>();
}

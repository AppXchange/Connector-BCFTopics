namespace Connector.BCF21.v1.CommentsBatch;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents the batch response from BCF 2.1 comments batch operations
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("Represents the batch response from BCF 2.1 comments batch operations")]
public class CommentsBatchDataObject
{
    [JsonPropertyName("items")]
    [Description("Collection of comments in the batch")]
    public CommentItem[] Items { get; init; } = Array.Empty<CommentItem>();

    [JsonPropertyName("errors")]
    [Description("Collection of errors that occurred during batch processing")]
    public BatchError[] Errors { get; init; } = Array.Empty<BatchError>();
}

public class CommentItem
{
    [JsonPropertyName("version")]
    public int Version { get; init; }

    [JsonPropertyName("guid")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("date")]
    public DateTime Date { get; init; }

    [JsonPropertyName("author")]
    public string Author { get; init; } = string.Empty;

    [JsonPropertyName("author_uuid")]
    public string AuthorUuid { get; init; } = string.Empty;

    [JsonPropertyName("created_by_uuid")]
    public string CreatedByUuid { get; init; } = string.Empty;

    [JsonPropertyName("comment")]
    public string Comment { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;

    [JsonPropertyName("viewpoint_guid")]
    public string? ViewpointGuid { get; init; }

    [JsonPropertyName("viewpoint")]
    public Viewpoint? Viewpoint { get; init; }

    [JsonPropertyName("reply_to_comment_guid")]
    public string? ReplyToCommentGuid { get; init; }

    [JsonPropertyName("modified_date")]
    public DateTime ModifiedDate { get; init; }

    [JsonPropertyName("modified_author")]
    public string ModifiedAuthor { get; init; } = string.Empty;

    [JsonPropertyName("modified_author_uuid")]
    public string ModifiedAuthorUuid { get; init; } = string.Empty;
}

public class Viewpoint
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("view_id")]
    public string ViewId { get; init; } = string.Empty;

    [JsonPropertyName("snapshot_url")]
    public string SnapshotUrl { get; init; } = string.Empty;

    [JsonPropertyName("snapshot_thumb")]
    public string SnapshotThumb { get; init; } = string.Empty;
}

public class BatchError
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("item")]
    public CommentItem? Item { get; init; }
}
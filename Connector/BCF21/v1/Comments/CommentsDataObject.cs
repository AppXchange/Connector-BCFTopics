namespace Connector.BCF21.v1.Comments.Models;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("BCF 2.1 Comment object representing a comment on a topic")]
public class CommentsDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the comment")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("version")]
    [Description("Version of the comment")]
    public int Version { get; init; }

    [JsonPropertyName("date")]
    [Description("The date when the comment was created")]
    [Required]
    public required DateTime Date { get; init; }

    [JsonPropertyName("author")]
    [Description("The author of the comment")]
    [Required]
    public required string Author { get; init; }

    [JsonPropertyName("author_uuid")]
    [Description("UUID of the author")]
    public string? AuthorUuid { get; init; }

    [JsonPropertyName("created_by_uuid")]
    [Description("UUID of the user who created the comment")]
    public string? CreatedByUuid { get; init; }

    [JsonPropertyName("comment")]
    [Description("The comment text")]
    [Required]
    public required string Comment { get; init; }

    [JsonPropertyName("topic_guid")]
    [Description("The guid of the topic this comment belongs to")]
    [Required]
    public required string TopicGuid { get; init; }

    [JsonPropertyName("viewpoint_guid")]
    [Description("The guid of the viewpoint this comment references")]
    public string? ViewpointGuid { get; init; }

    [JsonPropertyName("viewpoint")]
    [Description("The viewpoint information")]
    public ViewpointInfo? Viewpoint { get; init; }

    [JsonPropertyName("reply_to_comment_guid")]
    [Description("The guid of the comment this is replying to")]
    public string? ReplyToCommentGuid { get; init; }

    [JsonPropertyName("modified_date")]
    [Description("The date when the comment was last modified")]
    public DateTime? ModifiedDate { get; init; }

    [JsonPropertyName("modified_author")]
    [Description("The author who last modified the comment")]
    public string? ModifiedAuthor { get; init; }

    [JsonPropertyName("modified_author_uuid")]
    [Description("UUID of the author who modified the comment")]
    public string? ModifiedAuthorUuid { get; init; }
}

public class ViewpointInfo
{
    [JsonPropertyName("guid")]
    public string? Guid { get; init; }

    [JsonPropertyName("view_id")]
    public string? ViewId { get; init; }

    [JsonPropertyName("snapshot_url")]
    public string? SnapshotUrl { get; init; }

    [JsonPropertyName("snapshot_thumb")]
    public string? SnapshotThumb { get; init; }
}
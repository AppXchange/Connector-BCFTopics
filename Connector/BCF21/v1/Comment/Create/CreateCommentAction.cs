namespace Connector.BCF21.v1.Comment.Create;

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
[Description("Creates a new comment for a specified topic in BCF 2.1")]
public class CreateCommentAction : IStandardAction<CreateCommentActionInput, CreateCommentActionOutput>
{
    public CreateCommentActionInput ActionInput { get; set; } = new();
    public CreateCommentActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateCommentActionInput
{
    [JsonPropertyName("projectId")]
    [Description("The id of the Trimble Connect Project")]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("topicId")]
    [Description("The id of the Topic")]
    public string TopicId { get; set; } = string.Empty;

    [JsonPropertyName("comment")]
    [Description("The comment text")]
    public string Comment { get; set; } = string.Empty;

    [JsonPropertyName("guid")]
    [Description("The unique identifier for the comment")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    [Description("The date when the comment was created")]
    public DateTime Date { get; set; }

    [JsonPropertyName("author")]
    [Description("The author of the comment")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("author_uuid")]
    [Description("The unique identifier of the author")]
    public string AuthorUuid { get; set; } = string.Empty;

    [JsonPropertyName("viewpoint_guid")]
    [Description("The unique identifier of the associated viewpoint")]
    public string? ViewpointGuid { get; set; }

    [JsonPropertyName("reply_to_comment_guid")]
    [Description("The unique identifier of the comment being replied to")]
    public string? ReplyToCommentGuid { get; set; }
}

public class CreateCommentActionOutput
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("guid")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("author_uuid")]
    public string AuthorUuid { get; set; } = string.Empty;

    [JsonPropertyName("created_by_uuid")]
    public string CreatedByUuid { get; set; } = string.Empty;

    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; set; } = string.Empty;

    [JsonPropertyName("viewpoint_guid")]
    public string? ViewpointGuid { get; set; }

    [JsonPropertyName("viewpoint")]
    public ViewpointInfo? Viewpoint { get; set; }

    [JsonPropertyName("reply_to_comment_guid")]
    public string? ReplyToCommentGuid { get; set; }

    [JsonPropertyName("modified_date")]
    public DateTime? ModifiedDate { get; set; }

    [JsonPropertyName("modified_author")]
    public string? ModifiedAuthor { get; set; }

    [JsonPropertyName("modified_author_uuid")]
    public string? ModifiedAuthorUuid { get; set; }
}

public class ViewpointInfo
{
    [JsonPropertyName("guid")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("view_id")]
    public string ViewId { get; set; } = string.Empty;

    [JsonPropertyName("snapshot_url")]
    public string SnapshotUrl { get; set; } = string.Empty;

    [JsonPropertyName("snapshot_thumb")]
    public string SnapshotThumb { get; set; } = string.Empty;
}

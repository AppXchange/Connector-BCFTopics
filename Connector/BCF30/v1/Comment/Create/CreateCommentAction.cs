namespace Connector.BCF30.v1.Comment.Create;

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
[Description("Creates a new BCF 3.0 comment on a topic")]
public class CreateCommentAction : IStandardAction<CreateCommentActionInput, CreateCommentActionOutput>
{
    public CreateCommentActionInput ActionInput { get; set; } = null!;
    public CreateCommentActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateCommentActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The id of the topic to add the comment to")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("comment")]
    [Description("The comment text")]
    [Required]
    public required string Comment { get; init; }

    [JsonPropertyName("guid")]
    [Description("Optional GUID for the comment. If not provided, one will be generated.")]
    public string? Guid { get; init; }
}

public class CreateCommentActionOutput
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the created comment")]
    public required string Guid { get; init; }

    [JsonPropertyName("date")]
    [Description("The date when the comment was created")]
    public required DateTime Date { get; init; }

    [JsonPropertyName("author")]
    [Description("The author of the comment")]
    public required string Author { get; init; }

    [JsonPropertyName("comment")]
    [Description("The comment text")]
    public required string Comment { get; init; }

    [JsonPropertyName("topic_guid")]
    [Description("The guid of the topic this comment belongs to")]
    public required string TopicGuid { get; init; }
}

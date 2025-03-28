namespace Connector.BCF30.v1.Comment.Models;

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
[Description("BCF 3.0 Comment object representing a single comment")]
public class CommentDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the comment")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("date")]
    [Description("The date when the comment was created")]
    [Required]
    public required DateTime Date { get; init; }

    [JsonPropertyName("author")]
    [Description("The author of the comment")]
    [Required]
    public required string Author { get; init; }

    [JsonPropertyName("comment")]
    [Description("The comment text")]
    [Required]
    public required string Comment { get; init; }

    [JsonPropertyName("topic_guid")]
    [Description("The guid of the topic this comment belongs to")]
    [Required]
    public required string TopicGuid { get; init; }
}
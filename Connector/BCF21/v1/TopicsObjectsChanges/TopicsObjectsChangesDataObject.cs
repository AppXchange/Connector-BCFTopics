namespace Connector.BCF21.v1.TopicsObjectsChanges;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents the changes response from BCF 2.1
/// </summary>
[PrimaryKey("changeToken", nameof(ChangeToken))]
[Description("Represents the changes response from BCF 2.1")]
public class TopicsObjectsChangesDataObject
{
    [JsonPropertyName("changeToken")]
    [Description("Token indicating the current state of the data")]
    [Required]
    public required string ChangeToken { get; init; }

    [JsonPropertyName("items")]
    [Description("Collection of items by type")]
    public Items Items { get; init; } = new();

    [JsonPropertyName("deletedItems")]
    [Description("Collection of deleted items by type")]
    public DeletedItems DeletedItems { get; init; } = new();

    [JsonPropertyName("links")]
    [Description("Navigation links for pagination")]
    public Links Links { get; init; } = new();
}

public class Items
{
    [JsonPropertyName("comments")]
    public Comment[] Comments { get; init; } = Array.Empty<Comment>();

    [JsonPropertyName("topics")]
    public Topic[] Topics { get; init; } = Array.Empty<Topic>();

    [JsonPropertyName("viewpoints")]
    public Viewpoint[] Viewpoints { get; init; } = Array.Empty<Viewpoint>();

    [JsonPropertyName("documentReferences")]
    public DocumentReference[] DocumentReferences { get; init; } = Array.Empty<DocumentReference>();
}

public class DeletedItems
{
    [JsonPropertyName("comments")]
    public DeletedComment[] Comments { get; init; } = Array.Empty<DeletedComment>();

    [JsonPropertyName("topics")]
    public DeletedTopic[] Topics { get; init; } = Array.Empty<DeletedTopic>();

    [JsonPropertyName("viewpoints")]
    public DeletedViewpoint[] Viewpoints { get; init; } = Array.Empty<DeletedViewpoint>();

    [JsonPropertyName("documentReferences")]
    public DeletedDocumentReference[] DocumentReferences { get; init; } = Array.Empty<DeletedDocumentReference>();
}

public class Comment
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; init; }

    [JsonPropertyName("author")]
    public string Author { get; init; } = string.Empty;

    [JsonPropertyName("author_uuid")]
    public string AuthorUuid { get; init; } = string.Empty;

    [JsonPropertyName("comment")]
    public string CommentText { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class Topic
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class Viewpoint
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class DocumentReference
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class DeletedComment
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class DeletedTopic
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class DeletedViewpoint
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class DeletedDocumentReference
{
    [JsonPropertyName("guid")]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("topic_guid")]
    public string TopicGuid { get; init; } = string.Empty;
}

public class Links
{
    [JsonPropertyName("self")]
    public Link Self { get; init; } = new();

    [JsonPropertyName("first")]
    public Link First { get; init; } = new();

    [JsonPropertyName("next")]
    public Link Next { get; init; } = new();
}

public class Link
{
    [JsonPropertyName("href")]
    public string Href { get; init; } = string.Empty;
}
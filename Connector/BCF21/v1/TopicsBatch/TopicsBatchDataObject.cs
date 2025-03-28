namespace Connector.BCF21.v1.TopicsBatch;

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
[Description("Represents a batch of topics in BCF 2.1")]
public class TopicsBatchDataObject
{
    [JsonPropertyName("items")]
    [Description("Collection of topics in the batch")]
    public TopicItem[] Items { get; init; } = Array.Empty<TopicItem>();

    [JsonPropertyName("errors")]
    [Description("Collection of errors that occurred during batch processing")]
    public BatchError[] Errors { get; init; } = Array.Empty<BatchError>();
}

public class TopicItem
{
    [JsonPropertyName("version")]
    public int Version { get; init; }

    [JsonPropertyName("guid")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("topic_type")]
    public string TopicType { get; init; } = string.Empty;

    [JsonPropertyName("topic_status")]
    public string TopicStatus { get; init; } = string.Empty;

    [JsonPropertyName("reference_links")]
    public string[] ReferenceLinks { get; init; } = Array.Empty<string>();

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("priority")]
    public string Priority { get; init; } = string.Empty;

    [JsonPropertyName("index")]
    public int Index { get; init; }

    [JsonPropertyName("labels")]
    public string[] Labels { get; init; } = Array.Empty<string>();

    [JsonPropertyName("creation_date")]
    public DateTime CreationDate { get; init; }

    [JsonPropertyName("creation_author")]
    public string CreationAuthor { get; init; } = string.Empty;

    [JsonPropertyName("creation_author_uuid")]
    public string CreationAuthorUuid { get; init; } = string.Empty;

    [JsonPropertyName("created_by_uuid")]
    public string CreatedByUuid { get; init; } = string.Empty;

    [JsonPropertyName("modified_date")]
    public DateTime ModifiedDate { get; init; }

    [JsonPropertyName("modified_author")]
    public string ModifiedAuthor { get; init; } = string.Empty;

    [JsonPropertyName("modified_author_uuid")]
    public string ModifiedAuthorUuid { get; init; } = string.Empty;

    [JsonPropertyName("assigned_to")]
    public string AssignedTo { get; init; } = string.Empty;

    [JsonPropertyName("assigned_to_uuid")]
    public string AssignedToUuid { get; init; } = string.Empty;

    [JsonPropertyName("assignees")]
    public Assignee[] Assignees { get; init; } = Array.Empty<Assignee>();

    [JsonPropertyName("stage")]
    public string Stage { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; init; }

    [JsonPropertyName("marker")]
    public Marker Marker { get; init; } = new();

    [JsonPropertyName("viewpoint")]
    public Viewpoint Viewpoint { get; init; } = new();

    [JsonPropertyName("bim_snippet")]
    public BimSnippet BimSnippet { get; init; } = new();

    [JsonPropertyName("related_topics")]
    public RelatedTopic[] RelatedTopics { get; init; } = Array.Empty<RelatedTopic>();

    [JsonPropertyName("files")]
    public File[] Files { get; init; } = Array.Empty<File>();
}

public class BatchError
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("item")]
    public TopicItem? Item { get; init; }
}

public class Assignee
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;
}

public class Marker
{
    [JsonPropertyName("model_point")]
    public ModelPoint ModelPoint { get; init; } = new();
}

public class ModelPoint
{
    [JsonPropertyName("x")]
    public double X { get; init; }

    [JsonPropertyName("y")]
    public double Y { get; init; }

    [JsonPropertyName("z")]
    public double Z { get; init; }
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

public class BimSnippet
{
    [JsonPropertyName("snippet_type")]
    public string SnippetType { get; init; } = string.Empty;

    [JsonPropertyName("is_external")]
    public bool IsExternal { get; init; }

    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;

    [JsonPropertyName("reference_schema")]
    public string ReferenceSchema { get; init; } = string.Empty;
}

public class RelatedTopic
{
    [JsonPropertyName("related_topic_guid")]
    public string RelatedTopicGuid { get; init; } = string.Empty;
}

public class File
{
    [JsonPropertyName("ifc_project")]
    public string IfcProject { get; init; } = string.Empty;

    [JsonPropertyName("ifc_spatial_structure_element")]
    public string IfcSpatialStructureElement { get; init; } = string.Empty;

    [JsonPropertyName("file_name")]
    public string FileName { get; init; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; init; } = string.Empty;

    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;
}
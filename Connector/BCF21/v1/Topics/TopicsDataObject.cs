namespace Connector.BCF21.v1.Topics;

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
[Description("Represents a topic in BCF 2.1")]
public class TopicsDataObject
{
    [JsonPropertyName("version")]
    [Description("The version of the topic")]
    public int Version { get; init; }

    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the topic")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("topic_type")]
    [Description("The type of the topic")]
    public string TopicType { get; init; } = string.Empty;

    [JsonPropertyName("topic_status")]
    [Description("The status of the topic")]
    public string TopicStatus { get; init; } = string.Empty;

    [JsonPropertyName("reference_links")]
    [Description("Collection of reference links")]
    public string[] ReferenceLinks { get; init; } = Array.Empty<string>();

    [JsonPropertyName("title")]
    [Description("The title of the topic")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("priority")]
    [Description("The priority level of the topic")]
    public string Priority { get; init; } = string.Empty;

    [JsonPropertyName("index")]
    [Description("The index of the topic")]
    public int Index { get; init; }

    [JsonPropertyName("labels")]
    [Description("Collection of labels")]
    public string[] Labels { get; init; } = Array.Empty<string>();

    [JsonPropertyName("creation_date")]
    [Description("The date when the topic was created")]
    public DateTime CreationDate { get; init; }

    [JsonPropertyName("creation_author")]
    [Description("The author who created the topic")]
    public string CreationAuthor { get; init; } = string.Empty;

    [JsonPropertyName("creation_author_uuid")]
    [Description("The UUID of the author who created the topic")]
    public string CreationAuthorUuid { get; init; } = string.Empty;

    [JsonPropertyName("created_by_uuid")]
    [Description("The UUID of the user who created the topic")]
    public string CreatedByUuid { get; init; } = string.Empty;

    [JsonPropertyName("modified_date")]
    [Description("The date when the topic was last modified")]
    public DateTime ModifiedDate { get; init; }

    [JsonPropertyName("modified_author")]
    [Description("The author who last modified the topic")]
    public string ModifiedAuthor { get; init; } = string.Empty;

    [JsonPropertyName("modified_author_uuid")]
    [Description("The UUID of the author who last modified the topic")]
    public string ModifiedAuthorUuid { get; init; } = string.Empty;

    [JsonPropertyName("assigned_to")]
    [Description("The user assigned to the topic")]
    public string AssignedTo { get; init; } = string.Empty;

    [JsonPropertyName("assigned_to_uuid")]
    [Description("The UUID of the user assigned to the topic")]
    public string AssignedToUuid { get; init; } = string.Empty;

    [JsonPropertyName("assignees")]
    [Description("Collection of assignees")]
    public Assignee[] Assignees { get; init; } = Array.Empty<Assignee>();

    [JsonPropertyName("stage")]
    [Description("The stage of the topic")]
    public string Stage { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    [Description("The description of the topic")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("due_date")]
    [Description("The due date of the topic")]
    public DateTime? DueDate { get; init; }

    [JsonPropertyName("marker")]
    [Description("The marker information")]
    public Marker Marker { get; init; } = new();

    [JsonPropertyName("viewpoint")]
    [Description("The viewpoint information")]
    public Viewpoint Viewpoint { get; init; } = new();

    [JsonPropertyName("bim_snippet")]
    [Description("The BIM snippet information")]
    public BimSnippet BimSnippet { get; init; } = new();

    [JsonPropertyName("related_topics")]
    [Description("Collection of related topics")]
    public RelatedTopic[] RelatedTopics { get; init; } = Array.Empty<RelatedTopic>();

    [JsonPropertyName("files")]
    [Description("Collection of files")]
    public File[] Files { get; init; } = Array.Empty<File>();
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
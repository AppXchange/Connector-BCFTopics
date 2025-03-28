namespace Connector.BCF21.v1.Topic.Update;

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
[Description("Updates an existing Topic in a BCF 2.1 project")]
public class UpdateTopicAction : IStandardAction<UpdateTopicActionInput, UpdateTopicActionOutput>
{
    public UpdateTopicActionInput ActionInput { get; set; } = new();
    public UpdateTopicActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateTopicActionInput
{
    [JsonPropertyName("topic_type")]
    [Description("The type of the topic")]
    public string TopicType { get; set; } = string.Empty;

    [JsonPropertyName("topic_status")]
    [Description("The status of the topic")]
    public string TopicStatus { get; set; } = string.Empty;

    [JsonPropertyName("reference_links")]
    [Description("Collection of reference links")]
    public string[] ReferenceLinks { get; set; } = Array.Empty<string>();

    [JsonPropertyName("title")]
    [Description("The title of the topic")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("priority")]
    [Description("The priority level of the topic")]
    public string Priority { get; set; } = string.Empty;

    [JsonPropertyName("index")]
    [Description("The index of the topic")]
    public int Index { get; set; }

    [JsonPropertyName("labels")]
    [Description("Collection of labels")]
    public string[] Labels { get; set; } = Array.Empty<string>();

    [JsonPropertyName("assigned_to")]
    [Description("The user assigned to the topic")]
    public string? AssignedTo { get; set; }

    [JsonPropertyName("assigned_to_uuid")]
    [Description("The UUID of the user assigned to the topic")]
    public string? AssignedToUuid { get; set; }

    [JsonPropertyName("assignees")]
    [Description("Collection of assignees")]
    public Assignee[] Assignees { get; set; } = Array.Empty<Assignee>();

    [JsonPropertyName("stage")]
    [Description("The stage of the topic")]
    public string Stage { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    [Description("The description of the topic")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("due_date")]
    [Description("The due date of the topic")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("marker")]
    [Description("The marker information")]
    public Marker Marker { get; set; } = new();

    [JsonPropertyName("bim_snippet")]
    [Description("The BIM snippet information")]
    public BimSnippet BimSnippet { get; set; } = new();
}

public class UpdateTopicActionOutput
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("guid")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("topic_type")]
    public string TopicType { get; set; } = string.Empty;

    [JsonPropertyName("topic_status")]
    public string TopicStatus { get; set; } = string.Empty;

    [JsonPropertyName("reference_links")]
    public string[] ReferenceLinks { get; set; } = Array.Empty<string>();

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = string.Empty;

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("labels")]
    public string[] Labels { get; set; } = Array.Empty<string>();

    [JsonPropertyName("creation_date")]
    public DateTime CreationDate { get; set; }

    [JsonPropertyName("creation_author")]
    public string CreationAuthor { get; set; } = string.Empty;

    [JsonPropertyName("creation_author_uuid")]
    public string CreationAuthorUuid { get; set; } = string.Empty;

    [JsonPropertyName("created_by_uuid")]
    public string CreatedByUuid { get; set; } = string.Empty;

    [JsonPropertyName("modified_date")]
    public DateTime ModifiedDate { get; set; }

    [JsonPropertyName("modified_author")]
    public string ModifiedAuthor { get; set; } = string.Empty;

    [JsonPropertyName("modified_author_uuid")]
    public string ModifiedAuthorUuid { get; set; } = string.Empty;

    [JsonPropertyName("assigned_to")]
    public string AssignedTo { get; set; } = string.Empty;

    [JsonPropertyName("assigned_to_uuid")]
    public string AssignedToUuid { get; set; } = string.Empty;

    [JsonPropertyName("assignees")]
    public Assignee[] Assignees { get; set; } = Array.Empty<Assignee>();

    [JsonPropertyName("stage")]
    public string Stage { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("marker")]
    public Marker Marker { get; set; } = new();

    [JsonPropertyName("viewpoint")]
    public Viewpoint Viewpoint { get; set; } = new();

    [JsonPropertyName("bim_snippet")]
    public BimSnippet BimSnippet { get; set; } = new();

    [JsonPropertyName("related_topics")]
    public RelatedTopic[] RelatedTopics { get; set; } = Array.Empty<RelatedTopic>();

    [JsonPropertyName("files")]
    public File[] Files { get; set; } = Array.Empty<File>();
}

public class Assignee
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

public class Marker
{
    [JsonPropertyName("model_point")]
    public ModelPoint ModelPoint { get; set; } = new();
}

public class ModelPoint
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }
}

public class Viewpoint
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

public class BimSnippet
{
    [JsonPropertyName("snippet_type")]
    public string SnippetType { get; set; } = string.Empty;

    [JsonPropertyName("is_external")]
    public bool IsExternal { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("reference_schema")]
    public string ReferenceSchema { get; set; } = string.Empty;
}

public class RelatedTopic
{
    [JsonPropertyName("related_topic_guid")]
    public string RelatedTopicGuid { get; set; } = string.Empty;
}

public class File
{
    [JsonPropertyName("ifc_project")]
    public string IfcProject { get; set; } = string.Empty;

    [JsonPropertyName("ifc_spatial_structure_element")]
    public string IfcSpatialStructureElement { get; set; } = string.Empty;

    [JsonPropertyName("file_name")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;
}

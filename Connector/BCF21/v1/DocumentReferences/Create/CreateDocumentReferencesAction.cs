namespace Connector.BCF21.v1.DocumentReferences.Create;

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
[Description("Creates a new document reference for a topic in BCF 2.1")]
public class CreateDocumentReferencesAction : IStandardAction<CreateDocumentReferencesActionInput, CreateDocumentReferencesActionOutput>
{
    public CreateDocumentReferencesActionInput ActionInput { get; set; } = new();
    public CreateDocumentReferencesActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateDocumentReferencesActionInput
{
    [JsonPropertyName("projectId")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("topicId")]
    [Description("The id of the Topic")]
    [Required]
    public string TopicId { get; set; } = string.Empty;

    [JsonPropertyName("guid")]
    [Description("The unique identifier of the document reference")]
    public string? Guid { get; set; }

    [JsonPropertyName("document_guid")]
    [Description("The unique identifier of the referenced document")]
    public string? DocumentGuid { get; set; }

    [JsonPropertyName("url")]
    [Description("The URL of the external document")]
    public string? Url { get; set; }

    [JsonPropertyName("description")]
    [Description("The description of the document reference")]
    public string? Description { get; set; }
}

public class CreateDocumentReferencesActionOutput
{
    [JsonPropertyName("version")]
    [Description("The version of the document reference")]
    public int Version { get; set; }

    [JsonPropertyName("guid")]
    [Description("The unique identifier of the document reference")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("document_guid")]
    [Description("The unique identifier of the referenced document")]
    public string? DocumentGuid { get; set; }

    [JsonPropertyName("url")]
    [Description("The URL of the external document")]
    public string? Url { get; set; }

    [JsonPropertyName("description")]
    [Description("The description of the document reference")]
    public string? Description { get; set; }

    [JsonPropertyName("created_by_uuid")]
    [Description("The unique identifier of the user who created the document reference")]
    public string? CreatedByUuid { get; set; }

    [JsonPropertyName("type")]
    [Description("The type of the document reference")]
    public string? Type { get; set; }
}

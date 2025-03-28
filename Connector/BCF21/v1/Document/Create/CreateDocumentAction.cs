namespace Connector.BCF21.v1.Document.Create;

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
[Description("Creates a new document in a BCF 2.1 project")]
public class CreateDocumentAction : IStandardAction<CreateDocumentActionInput, CreateDocumentActionOutput>
{
    public CreateDocumentActionInput ActionInput { get; set; } = new();
    public CreateDocumentActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateDocumentActionInput
{
    [JsonPropertyName("projectId")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    [Required]
    public string Filename { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    [Description("The binary content of the document")]
    [Required]
    public byte[] Content { get; set; } = Array.Empty<byte>();
}

public class CreateDocumentActionOutput
{
    [JsonPropertyName("guid")]
    [Description("The unique identifier of the created document")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    public string Filename { get; set; } = string.Empty;
}

namespace Connector.BCF30.v1.Document.Create;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for creating a new document in BCF 3.0
/// </summary>
[Description("Creates a new document in a BCF 3.0 project")]
public class CreateDocumentAction : IStandardAction<CreateDocumentActionInput, CreateDocumentActionOutput>
{
    public CreateDocumentActionInput ActionInput { get; set; } = null!;
    public CreateDocumentActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateDocumentActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    [Required]
    public required string Filename { get; init; }

    [JsonPropertyName("content")]
    [Description("The binary content of the document")]
    [Required]
    public required byte[] Content { get; init; }
}

public class CreateDocumentActionOutput
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the created document")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    [Required]
    public required string Filename { get; init; }
}

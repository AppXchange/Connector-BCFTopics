namespace Connector.BCF30.v1.DocumentReferences.Update;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for updating a document reference in BCF 3.0
/// </summary>
[Description("Updates an existing document reference in a BCF 3.0 topic")]
public class UpdateDocumentReferencesAction : IStandardAction<UpdateDocumentReferencesActionInput, UpdateDocumentReferencesActionOutput>
{
    public UpdateDocumentReferencesActionInput ActionInput { get; set; } = null!;
    public UpdateDocumentReferencesActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateDocumentReferencesActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The id of the topic containing the document reference")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("document_reference_id")]
    [Description("The id of the document reference to update")]
    [Required]
    public required string DocumentReferenceId { get; init; }

    [JsonPropertyName("url")]
    [Description("The URL to the external document (required for external documents, must be null for internal documents)")]
    public string? Url { get; init; }

    [JsonPropertyName("document_guid")]
    [Description("The guid of the internal document (required for internal documents, must be null for external documents)")]
    public string? DocumentGuid { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the document reference")]
    [Required]
    public required string Description { get; init; }
}

public class UpdateDocumentReferencesActionOutput
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the document reference")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("url")]
    [Description("The URL to the external document")]
    public string? Url { get; init; }

    [JsonPropertyName("document_guid")]
    [Description("The guid of the internal document")]
    public string? DocumentGuid { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the document reference")]
    [Required]
    public required string Description { get; init; }
}

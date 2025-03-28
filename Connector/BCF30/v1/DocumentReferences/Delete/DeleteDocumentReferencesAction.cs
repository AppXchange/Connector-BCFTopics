namespace Connector.BCF30.v1.DocumentReferences.Delete;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for deleting a document reference in BCF 3.0
/// </summary>
[Description("Deletes an existing document reference in a BCF 3.0 topic")]
public class DeleteDocumentReferencesAction : IStandardAction<DeleteDocumentReferencesActionInput, DeleteDocumentReferencesActionOutput>
{
    public DeleteDocumentReferencesActionInput ActionInput { get; set; } = null!;
    public DeleteDocumentReferencesActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class DeleteDocumentReferencesActionInput
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
    [Description("The id of the document reference to delete")]
    [Required]
    public required string DocumentReferenceId { get; init; }
}

public class DeleteDocumentReferencesActionOutput
{
    // No output properties needed for delete operation as it returns 204 No Content
}

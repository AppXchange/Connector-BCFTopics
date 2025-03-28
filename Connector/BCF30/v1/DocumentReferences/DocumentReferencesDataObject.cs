namespace Connector.BCF30.v1.DocumentReferences.Models;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a document reference in BCF 3.0
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("BCF 3.0 Document Reference object representing a reference to a document in a topic")]
public class DocumentReferencesDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the document reference")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("url")]
    [Description("The URL to the referenced document")]
    public string? Url { get; init; }

    [JsonPropertyName("document_guid")]
    [Description("The guid of the referenced document")]
    public string? DocumentGuid { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the document reference")]
    [Required]
    public required string Description { get; init; }
}
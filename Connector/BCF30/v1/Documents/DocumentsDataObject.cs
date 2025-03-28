namespace Connector.BCF30.v1.Documents.Models;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a document in BCF 3.0
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("BCF 3.0 Document object representing a document in a project")]
public class DocumentsDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the document")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    [Required]
    public required string Filename { get; init; }
}
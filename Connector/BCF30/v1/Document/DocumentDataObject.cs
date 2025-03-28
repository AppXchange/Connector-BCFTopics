namespace Connector.BCF30.v1.Document.Models;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a single document in BCF 3.0
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("BCF 3.0 Document object representing a single document in a project")]
public class DocumentDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the document")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    [Required]
    public required string Filename { get; init; }

    [JsonPropertyName("content")]
    [Description("The binary content of the document")]
    [Required]
    public required byte[] Content { get; init; }
}
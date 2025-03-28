namespace Connector.BCF21.v1.DocumentReferences;

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
[Description("Represents a document reference in a BCF 2.1 topic")]
public class DocumentReferencesDataObject
{
    [JsonPropertyName("version")]
    [Description("The version of the document reference")]
    public int Version { get; init; }

    [JsonPropertyName("guid")]
    [Description("The unique identifier of the document reference")]
    [Required]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("document_guid")]
    [Description("The unique identifier of the referenced document")]
    [Required]
    public string DocumentGuid { get; init; } = string.Empty;

    [JsonPropertyName("url")]
    [Description("The URL of the document")]
    public string? Url { get; init; }

    [JsonPropertyName("description")]
    [Description("The description of the document reference")]
    public string? Description { get; init; }

    [JsonPropertyName("created_by_uuid")]
    [Description("The unique identifier of the user who created the document reference")]
    public string? CreatedByUuid { get; init; }

    [JsonPropertyName("type")]
    [Description("The type of the document reference")]
    public string? Type { get; init; }
}
namespace Connector.BCF21.v1.Documents;

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
[Description("Represents a document in a BCF 2.1 project")]
public class DocumentsDataObject
{
    [JsonPropertyName("guid")]
    [Description("The unique identifier of the document")]
    [Required]
    public string Guid { get; init; } = string.Empty;

    [JsonPropertyName("filename")]
    [Description("The name of the document file")]
    [Required]
    public string Filename { get; init; } = string.Empty;
}
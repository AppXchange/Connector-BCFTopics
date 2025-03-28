namespace Connector.BCF30.v1.Files;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a file reference in a BCF 3.0 topic
/// </summary>
[PrimaryKey("reference", nameof(Reference))]
[Description("BCF 3.0 File object representing a file reference in a topic")]
public class FilesDataObject
{
    [JsonPropertyName("ifc_project")]
    [Description("The IFC project identifier")]
    [Required]
    public required string IfcProject { get; init; }

    [JsonPropertyName("filename")]
    [Description("The name of the referenced file")]
    [Required]
    public required string Filename { get; init; }

    [JsonPropertyName("reference")]
    [Description("The reference to the file, either as an absolute URI or a server-specific ID")]
    [Required]
    public required string Reference { get; init; }
}
namespace Connector.BCF21.v1.Files;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Represents a file reference in a BCF 2.1 topic
/// </summary>
[PrimaryKey("reference", nameof(Reference))]
[Description("Represents a file reference in a BCF 2.1 topic")]
public class FilesDataObject
{
    [JsonPropertyName("ifc_project")]
    [Description("The IFC project identifier")]
    public string? IfcProject { get; init; }

    [JsonPropertyName("ifc_spatial_structure_element")]
    [Description("The IFC spatial structure element")]
    public string? IfcSpatialStructureElement { get; init; }

    [JsonPropertyName("file_name")]
    [Description("The name of the referenced file")]
    [Required]
    public string FileName { get; init; } = string.Empty;

    [JsonPropertyName("date")]
    [Description("The date associated with the file")]
    [Required]
    public string Date { get; init; } = string.Empty;

    [JsonPropertyName("reference")]
    [Description("The unique reference identifier for the file")]
    [Required]
    public string Reference { get; init; } = string.Empty;
}
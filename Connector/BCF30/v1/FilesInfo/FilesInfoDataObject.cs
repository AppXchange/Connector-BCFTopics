namespace Connector.BCF30.v1.FilesInfo;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing file information in BCF 3.0
/// </summary>
[PrimaryKey("file.reference", "File.Reference")]
[Description("BCF 3.0 File Information object representing file metadata and display information")]
public class FilesInfoDataObject
{
    [JsonPropertyName("display_information")]
    [Description("Collection of display information fields for the file")]
    [Required]
    public required IEnumerable<DisplayInformation> DisplayInformation { get; init; }

    [JsonPropertyName("file")]
    [Description("The file reference information")]
    [Required]
    public required FileInfo File { get; init; }
}

public class DisplayInformation
{
    [JsonPropertyName("field_display_name")]
    [Description("The display name of the field")]
    [Required]
    public required string FieldDisplayName { get; init; }

    [JsonPropertyName("field_value")]
    [Description("The value of the field")]
    [Required]
    public required string FieldValue { get; init; }
}

public class FileInfo
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
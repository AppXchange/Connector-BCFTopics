namespace Connector.BCF21.v1.Files.Update;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Updates the referenced files of a BCF 2.1 topic
/// </summary>
[Description("Updates the referenced files of a BCF 2.1 topic")]
public class UpdateFilesAction : IStandardAction<UpdateFilesActionInput, UpdateFilesActionOutput>
{
    public UpdateFilesActionInput ActionInput { get; set; } = new();
    public UpdateFilesActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateFilesActionInput
{
    [JsonPropertyName("projectId")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("topicId")]
    [Description("The id of the Topic")]
    [Required]
    public string TopicId { get; set; } = string.Empty;

    [JsonPropertyName("files")]
    [Description("The array of files to be updated")]
    [Required]
    public List<FileReference> Files { get; set; } = new();
}

public class FileReference
{
    [JsonPropertyName("ifc_project")]
    [Description("The IFC project identifier")]
    public string? IfcProject { get; set; }

    [JsonPropertyName("ifc_spatial_structure_element")]
    [Description("The IFC spatial structure element")]
    public string? IfcSpatialStructureElement { get; set; }

    [JsonPropertyName("file_name")]
    [Description("The name of the referenced file")]
    [Required]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    [Description("The date associated with the file")]
    public string? Date { get; set; }

    [JsonPropertyName("reference")]
    [Description("The unique reference identifier for the file")]
    [Required]
    public string Reference { get; set; } = string.Empty;
}

public class UpdateFilesActionOutput
{
    [JsonPropertyName("files")]
    [Description("The updated array of files")]
    public List<FileReference> Files { get; set; } = new();
}

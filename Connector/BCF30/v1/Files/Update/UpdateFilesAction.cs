namespace Connector.BCF30.v1.Files.Update;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for updating files in a BCF 3.0 topic
/// </summary>
[Description("Updates the collection of file references in a BCF 3.0 topic")]
public class UpdateFilesAction : IStandardAction<UpdateFilesActionInput, UpdateFilesActionOutput>
{
    public UpdateFilesActionInput ActionInput { get; set; } = null!;
    public UpdateFilesActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UpdateFilesActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The id of the Trimble Connect Project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The id of the topic containing the files")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("files")]
    [Description("The collection of file references to update")]
    [Required]
    public required IEnumerable<FileReference> Files { get; init; }
}

public class FileReference
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

public class UpdateFilesActionOutput
{
    [JsonPropertyName("files")]
    [Description("The updated collection of file references")]
    [Required]
    public required IEnumerable<FileReference> Files { get; init; }
}

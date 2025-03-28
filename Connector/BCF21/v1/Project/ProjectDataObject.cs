namespace Connector.BCF21.v1.Project;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Represents a specific project in BCF 2.1
/// </summary>
[PrimaryKey("project_id", nameof(ProjectId))]
[Description("Represents a specific project in BCF 2.1")]
public class ProjectDataObject
{
    [JsonPropertyName("project_id")]
    [Description("The unique identifier of the project")]
    [Required]
    public string ProjectId { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    [Description("The name of the project")]
    [Required]
    public string Name { get; init; } = string.Empty;
}
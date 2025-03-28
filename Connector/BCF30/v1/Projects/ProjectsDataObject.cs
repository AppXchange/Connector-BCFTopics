namespace Connector.BCF30.v1.Projects;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object representing a project in BCF 3.0
/// </summary>
[PrimaryKey("project_id", nameof(ProjectId))]
[Description("BCF 3.0 Project object representing a project with its authorization information")]
public class ProjectsDataObject
{
    [JsonPropertyName("project_id")]
    [Description("The globally unique identifier of the project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("name")]
    [Description("The name of the project")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("authorization")]
    [Description("The authorization information for the project")]
    [Required]
    public required Authorization Authorization { get; init; }
}

public class Authorization
{
    [JsonPropertyName("project_actions")]
    [Description("List of actions the current user is authorized to perform")]
    [Required]
    public required IEnumerable<string> ProjectActions { get; init; }
}
namespace Connector.BCF30.v1.Project;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;
using System.Collections.Generic;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("project_id", nameof(ProjectId))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("BCF 3.0 Project object representing a single project with its authorization information")]
public class ProjectDataObject
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
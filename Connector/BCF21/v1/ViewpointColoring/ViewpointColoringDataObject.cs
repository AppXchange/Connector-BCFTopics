namespace Connector.BCF21.v1.ViewpointColoring;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents a BCF 2.1 viewpoint coloring
/// </summary>
[PrimaryKey("viewpoint_id", nameof(ViewpointId))]
[Description("Represents a BCF 2.1 viewpoint coloring")]
public class ViewpointColoringDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The identifier of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("coloring")]
    [Description("Collection of colored components in the viewpoint")]
    [Required]
    public required Coloring[] Coloring { get; init; }
}

public class Coloring
{
    [JsonPropertyName("color")]
    [Description("The color value")]
    [Required]
    public required string Color { get; init; }

    [JsonPropertyName("components")]
    [Description("Collection of components with this color")]
    [Required]
    public required Component[] Components { get; init; }
}

public class Component
{
    [JsonPropertyName("ifc_guid")]
    [Description("The IFC GUID of the component")]
    [Required]
    public required string IfcGuid { get; init; }

    [JsonPropertyName("originating_system")]
    [Description("The originating system")]
    public string? OriginatingSystem { get; init; }

    [JsonPropertyName("authoring_tool_id")]
    [Description("The authoring tool ID")]
    public string? AuthoringToolId { get; init; }
}
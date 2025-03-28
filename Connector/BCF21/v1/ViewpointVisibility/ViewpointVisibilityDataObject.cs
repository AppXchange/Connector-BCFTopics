namespace Connector.BCF21.v1.ViewpointVisibility;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents a BCF 2.1 viewpoint visibility
/// </summary>
[PrimaryKey("viewpoint_id", nameof(ViewpointId))]
[Description("Represents a BCF 2.1 viewpoint visibility")]
public class ViewpointVisibilityDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The identifier of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("visibility")]
    [Description("The visibility settings")]
    [Required]
    public required Visibility Visibility { get; init; }
}

public class Visibility
{
    [JsonPropertyName("default_visibility")]
    [Description("The default visibility state")]
    [Required]
    public required bool DefaultVisibility { get; init; }

    [JsonPropertyName("exceptions")]
    [Description("Collection of components that are exceptions to the default visibility")]
    public Component[] Exceptions { get; init; } = Array.Empty<Component>();

    [JsonPropertyName("view_setup_hints")]
    [Description("Additional view setup hints")]
    public ViewSetupHints? ViewSetupHints { get; init; }
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

public class ViewSetupHints
{
    [JsonPropertyName("spaces_visible")]
    [Description("Whether spaces are visible")]
    public bool? SpacesVisible { get; init; }

    [JsonPropertyName("space_boundaries_visible")]
    [Description("Whether space boundaries are visible")]
    public bool? SpaceBoundariesVisible { get; init; }

    [JsonPropertyName("openings_visible")]
    [Description("Whether openings are visible")]
    public bool? OpeningsVisible { get; init; }
}
namespace Connector.BCF30.v1.ViewpointVisibility;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("viewpoint_id", nameof(ViewpointId))]
[Description("BCF 3.0 Viewpoint Visibility object representing component visibility in a viewpoint")]
public class ViewpointVisibilityDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The ID of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("visibility")]
    [Description("Visibility settings for components")]
    [Required]
    public required ComponentVisibility Visibility { get; init; }
}

public class ComponentVisibility
{
    [JsonPropertyName("default_visibility")]
    [Description("Default visibility state for all components")]
    [Required]
    public required bool DefaultVisibility { get; init; }

    [JsonPropertyName("exceptions")]
    [Description("List of components with visibility state different from default_visibility")]
    public IEnumerable<ComponentException>? Exceptions { get; init; }

    [JsonPropertyName("view_setup_hints")]
    [Description("Additional visualization configurations")]
    public ViewSetupHints? ViewSetupHints { get; init; }
}

public class ComponentException
{
    [JsonPropertyName("ifc_guid")]
    [Description("The IFC GUID of the component")]
    [Required]
    public required string IfcGuid { get; init; }

    [JsonPropertyName("originating_system")]
    [Description("The system where the component originated")]
    public string? OriginatingSystem { get; init; }

    [JsonPropertyName("authoring_tool_id")]
    [Description("The tool that authored the component")]
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
namespace Connector.BCF30.v1.ViewpointSelection;

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
[PrimaryKey("viewpoint_id", nameof(ViewpointId))]
[Description("BCF 3.0 Viewpoint Selection object representing selected components in a viewpoint")]
public class ViewpointSelectionDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The ID of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("selection")]
    [Description("Collection of selected components")]
    [Required]
    public required IEnumerable<Component> Selection { get; init; }
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
namespace Connector.BCF21.v1.ViewpointSelection;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents a BCF 2.1 viewpoint selection
/// </summary>
[PrimaryKey("viewpoint_id", nameof(ViewpointId))]
[Description("Represents a BCF 2.1 viewpoint selection")]
public class ViewpointSelectionDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The identifier of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("selection")]
    [Description("Collection of selected components in the viewpoint")]
    [Required]
    public required Component[] Selection { get; init; }
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
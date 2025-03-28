namespace Connector.BCF30.v1.Viewpoint.Create;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object that will represent an action in the Xchange system. This will contain an input object type,
/// an output object type, and a Action failure type (this will default to <see cref="StandardActionFailure"/>
/// but that can be overridden with your own preferred type). These objects will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[Description("Creates a new viewpoint in BCF 3.0")]
public class CreateViewpointAction : IStandardAction<CreateViewpointActionInput, CreateViewpointActionOutput>
{
    public CreateViewpointActionInput ActionInput { get; set; } = null!;
    public CreateViewpointActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateViewpointActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project to create the viewpoint in")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The ID of the topic to create the viewpoint in")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("index")]
    [Description("The index of the viewpoint")]
    [Required]
    public required int Index { get; init; }

    [JsonPropertyName("perspective_camera")]
    [Description("The perspective camera settings")]
    [Required]
    public required PerspectiveCamera PerspectiveCamera { get; init; }

    [JsonPropertyName("lines")]
    [Description("Collection of lines to be drawn")]
    public IEnumerable<Line>? Lines { get; init; }

    [JsonPropertyName("clipping_planes")]
    [Description("Collection of clipping planes")]
    public IEnumerable<ClippingPlane>? ClippingPlanes { get; init; }

    [JsonPropertyName("snapshot")]
    [Description("Snapshot information")]
    public SnapshotInput? Snapshot { get; init; }

    [JsonPropertyName("components")]
    [Description("Component information")]
    public Components? Components { get; init; }
}

public class SnapshotInput
{
    [JsonPropertyName("snapshot_type")]
    [Required]
    public required string SnapshotType { get; init; }

    [JsonPropertyName("snapshot_data")]
    [Required]
    public required string SnapshotData { get; init; }
}

public class Components
{
    [JsonPropertyName("selection")]
    public IEnumerable<ComponentSelection>? Selection { get; init; }

    [JsonPropertyName("coloring")]
    public IEnumerable<ComponentColoring>? Coloring { get; init; }

    [JsonPropertyName("visibility")]
    public ComponentVisibility? Visibility { get; init; }
}

public class ComponentSelection
{
    [JsonPropertyName("ifc_guid")]
    [Required]
    public required string IfcGuid { get; init; }

    [JsonPropertyName("originating_system")]
    public string? OriginatingSystem { get; init; }

    [JsonPropertyName("authoring_tool_id")]
    public string? AuthoringToolId { get; init; }
}

public class ComponentColoring
{
    [JsonPropertyName("color")]
    [Required]
    public required string Color { get; init; }

    [JsonPropertyName("components")]
    [Required]
    public required IEnumerable<ComponentReference> Components { get; init; }
}

public class ComponentReference
{
    [JsonPropertyName("ifc_guid")]
    [Required]
    public required string IfcGuid { get; init; }
}

public class ComponentVisibility
{
    [JsonPropertyName("default_visibility")]
    [Required]
    public required bool DefaultVisibility { get; init; }

    [JsonPropertyName("exceptions")]
    public IEnumerable<ComponentReference>? Exceptions { get; init; }

    [JsonPropertyName("view_setup_hints")]
    public ViewSetupHints? ViewSetupHints { get; init; }
}

public class ViewSetupHints
{
    [JsonPropertyName("spaces_visible")]
    public bool? SpacesVisible { get; init; }

    [JsonPropertyName("space_boundaries_visible")]
    public bool? SpaceBoundariesVisible { get; init; }

    [JsonPropertyName("openings_visible")]
    public bool? OpeningsVisible { get; init; }
}

public class CreateViewpointActionOutput
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the created viewpoint")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("index")]
    [Description("The index of the viewpoint")]
    [Required]
    public required int Index { get; init; }

    [JsonPropertyName("perspective_camera")]
    [Description("The perspective camera settings")]
    public PerspectiveCamera? PerspectiveCamera { get; init; }

    [JsonPropertyName("lines")]
    [Description("Collection of lines to be drawn")]
    public IEnumerable<Line>? Lines { get; init; }

    [JsonPropertyName("clipping_planes")]
    [Description("Collection of clipping planes")]
    public IEnumerable<ClippingPlane>? ClippingPlanes { get; init; }

    [JsonPropertyName("snapshot")]
    [Description("Snapshot information")]
    public Snapshot? Snapshot { get; init; }
}

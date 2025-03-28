namespace Connector.BCF21.v1.Viewpoint;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents a BCF 2.1 viewpoint
/// </summary>
[PrimaryKey("guid", nameof(Guid))]
[Description("Represents a BCF 2.1 viewpoint")]
public class ViewpointDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the viewpoint")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("view_id")]
    [Description("The identifier of the view")]
    [Required]
    public required string ViewId { get; init; }

    [JsonPropertyName("index")]
    [Description("The index of the viewpoint")]
    [Required]
    public required int Index { get; init; }

    [JsonPropertyName("orthogonal_camera")]
    [Description("The orthogonal camera settings")]
    public OrthogonalCamera? OrthogonalCamera { get; init; }

    [JsonPropertyName("perspective_camera")]
    [Description("The perspective camera settings")]
    public PerspectiveCamera? PerspectiveCamera { get; init; }

    [JsonPropertyName("lines")]
    [Description("Collection of lines in the viewpoint")]
    public Line[] Lines { get; init; } = Array.Empty<Line>();

    [JsonPropertyName("clipping_planes")]
    [Description("Collection of clipping planes")]
    public ClippingPlane[] ClippingPlanes { get; init; } = Array.Empty<ClippingPlane>();

    [JsonPropertyName("bitmaps")]
    [Description("Collection of bitmaps")]
    public Bitmap[] Bitmaps { get; init; } = Array.Empty<Bitmap>();

    [JsonPropertyName("snapshot")]
    [Description("The snapshot information")]
    public Snapshot? Snapshot { get; init; }

    [JsonPropertyName("components")]
    [Description("The components information")]
    public Components? Components { get; init; }
}

public class OrthogonalCamera
{
    [JsonPropertyName("camera_view_point")]
    [Required]
    public required Point CameraViewPoint { get; init; }

    [JsonPropertyName("camera_direction")]
    [Required]
    public required Point CameraDirection { get; init; }

    [JsonPropertyName("camera_up_vector")]
    [Required]
    public required Point CameraUpVector { get; init; }

    [JsonPropertyName("view_to_world_scale")]
    [Required]
    public required double ViewToWorldScale { get; init; }
}

public class PerspectiveCamera
{
    [JsonPropertyName("camera_view_point")]
    [Required]
    public required Point CameraViewPoint { get; init; }

    [JsonPropertyName("camera_direction")]
    [Required]
    public required Point CameraDirection { get; init; }

    [JsonPropertyName("camera_up_vector")]
    [Required]
    public required Point CameraUpVector { get; init; }

    [JsonPropertyName("field_of_view")]
    [Required]
    public required double FieldOfView { get; init; }
}

public class Point
{
    [JsonPropertyName("x")]
    [Required]
    public required double X { get; init; }

    [JsonPropertyName("y")]
    [Required]
    public required double Y { get; init; }

    [JsonPropertyName("z")]
    [Required]
    public required double Z { get; init; }
}

public class Line
{
    [JsonPropertyName("start_point")]
    [Required]
    public required Point StartPoint { get; init; }

    [JsonPropertyName("end_point")]
    [Required]
    public required Point EndPoint { get; init; }
}

public class ClippingPlane
{
    [JsonPropertyName("location")]
    [Required]
    public required Point Location { get; init; }

    [JsonPropertyName("direction")]
    [Required]
    public required Point Direction { get; init; }
}

public class Bitmap
{
    [JsonPropertyName("guid")]
    [Required]
    public required string Guid { get; init; }

    [JsonPropertyName("bitmap_type")]
    [Required]
    public required string BitmapType { get; init; }

    [JsonPropertyName("bitmap_data")]
    [Required]
    public required string BitmapData { get; init; }

    [JsonPropertyName("location")]
    [Required]
    public required Point Location { get; init; }

    [JsonPropertyName("normal")]
    [Required]
    public required Point Normal { get; init; }

    [JsonPropertyName("up")]
    [Required]
    public required Point Up { get; init; }

    [JsonPropertyName("height")]
    [Required]
    public required double Height { get; init; }
}

public class Snapshot
{
    [JsonPropertyName("snapshot_type")]
    [Required]
    public required string SnapshotType { get; init; }

    [JsonPropertyName("snapshot_data")]
    [Required]
    public required string SnapshotData { get; init; }

    [JsonPropertyName("snapshot_url")]
    [Required]
    public required string SnapshotUrl { get; init; }
}

public class Components
{
    [JsonPropertyName("selection")]
    public Component[] Selection { get; init; } = Array.Empty<Component>();

    [JsonPropertyName("coloring")]
    public Coloring[] Coloring { get; init; } = Array.Empty<Coloring>();

    [JsonPropertyName("visibility")]
    public Visibility? Visibility { get; init; }
}

public class Component
{
    [JsonPropertyName("ifc_guid")]
    [Required]
    public required string IfcGuid { get; init; }

    [JsonPropertyName("originating_system")]
    public string? OriginatingSystem { get; init; }

    [JsonPropertyName("authoring_tool_id")]
    public string? AuthoringToolId { get; init; }
}

public class Coloring
{
    [JsonPropertyName("color")]
    [Required]
    public required string Color { get; init; }

    [JsonPropertyName("components")]
    [Required]
    public required Component[] Components { get; init; }
}

public class Visibility
{
    [JsonPropertyName("default_visibility")]
    [Required]
    public required bool DefaultVisibility { get; init; }

    [JsonPropertyName("exceptions")]
    public Component[] Exceptions { get; init; } = Array.Empty<Component>();

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
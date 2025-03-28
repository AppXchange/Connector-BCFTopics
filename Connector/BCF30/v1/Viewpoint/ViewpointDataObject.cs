namespace Connector.BCF30.v1.Viewpoint;

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
[PrimaryKey("guid", nameof(Guid))]
[Description("BCF 3.0 Viewpoint object representing a detailed view of a single viewpoint")]
public class ViewpointDataObject
{
    [JsonPropertyName("guid")]
    [Description("The globally unique identifier of the viewpoint")]
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

    [JsonPropertyName("aspect_ratio")]
    [Required]
    public required double AspectRatio { get; init; }
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

public class Snapshot
{
    [JsonPropertyName("snapshot_type")]
    [Required]
    public required string SnapshotType { get; init; }
}
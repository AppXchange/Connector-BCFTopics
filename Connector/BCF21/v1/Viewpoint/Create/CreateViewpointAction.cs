namespace Connector.BCF21.v1.Viewpoint.Create;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Creates a new Viewpoint for the specified Trimble Connect Project and Topic
/// </summary>
[Description("Creates a new Viewpoint for the specified Trimble Connect Project and Topic")]
public class CreateViewpointAction : IStandardAction<CreateViewpointActionInput, CreateViewpointActionOutput>
{
    public CreateViewpointActionInput ActionInput { get; set; } = new() { Index = 0 };
    public CreateViewpointActionOutput ActionOutput { get; set; } = new() { Guid = string.Empty, ViewId = string.Empty, Index = 0 };
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateViewpointActionInput
{
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
    public Line[]? Lines { get; init; }

    [JsonPropertyName("clipping_planes")]
    [Description("Collection of clipping planes")]
    public ClippingPlane[]? ClippingPlanes { get; init; }

    [JsonPropertyName("bitmaps")]
    [Description("Collection of bitmaps")]
    public Bitmap[]? Bitmaps { get; init; }

    [JsonPropertyName("snapshot")]
    [Description("The snapshot information")]
    public SnapshotInput? Snapshot { get; init; }

    [JsonPropertyName("components")]
    [Description("The components information")]
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

public class CreateViewpointActionOutput
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
    public Line[]? Lines { get; init; }

    [JsonPropertyName("clipping_planes")]
    [Description("Collection of clipping planes")]
    public ClippingPlane[]? ClippingPlanes { get; init; }

    [JsonPropertyName("bitmaps")]
    [Description("Collection of bitmaps")]
    public Bitmap[]? Bitmaps { get; init; }

    [JsonPropertyName("snapshot")]
    [Description("The snapshot information")]
    public Snapshot? Snapshot { get; init; }

    [JsonPropertyName("components")]
    [Description("The components information")]
    public Components? Components { get; init; }
}

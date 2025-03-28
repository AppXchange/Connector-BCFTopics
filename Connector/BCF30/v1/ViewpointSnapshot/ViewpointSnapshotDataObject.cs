namespace Connector.BCF30.v1.ViewpointSnapshot;

using Json.Schema.Generation;
using System;
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
[Description("BCF 3.0 Viewpoint Snapshot object representing a snapshot image of a viewpoint")]
public class ViewpointSnapshotDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The ID of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("snapshot_data")]
    [Description("The binary data of the snapshot image (PNG or JPG)")]
    [Required]
    public required byte[] SnapshotData { get; init; }

    [JsonPropertyName("snapshot_type")]
    [Description("The type of the snapshot (e.g. 'png' or 'jpg')")]
    [Required]
    public required string SnapshotType { get; init; }
}
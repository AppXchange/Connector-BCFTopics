namespace Connector.BCF21.v1.ViewpointSnapshot;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents a BCF 2.1 viewpoint snapshot
/// </summary>
[PrimaryKey("viewpoint_id", nameof(ViewpointId))]
[Description("Represents a BCF 2.1 viewpoint snapshot")]
public class ViewpointSnapshotDataObject
{
    [JsonPropertyName("viewpoint_id")]
    [Description("The identifier of the viewpoint")]
    [Required]
    public required string ViewpointId { get; init; }

    [JsonPropertyName("snapshot_data")]
    [Description("The binary data of the snapshot")]
    [Required]
    public required byte[] SnapshotData { get; init; }

    [JsonPropertyName("snapshot_type")]
    [Description("The type of the snapshot (e.g. png)")]
    [Required]
    public required string SnapshotType { get; init; }
}
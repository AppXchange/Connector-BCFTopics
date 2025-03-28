namespace Connector.BCF21.v1.Versions;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents a BCF version
/// </summary>
[PrimaryKey("version_id", nameof(VersionId))]
[Description("Represents a BCF version")]
public class VersionsDataObject
{
    [JsonPropertyName("version_id")]
    [Description("The identifier of the BCF version")]
    [Required]
    public required string VersionId { get; init; }

    [JsonPropertyName("detailed_version")]
    [Description("The detailed version information")]
    [Required]
    public required string DetailedVersion { get; init; }
}
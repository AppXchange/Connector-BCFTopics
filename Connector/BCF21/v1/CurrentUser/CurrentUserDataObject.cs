namespace Connector.BCF21.v1.CurrentUser;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents the current user in BCF 2.1
/// </summary>
[PrimaryKey("id", nameof(Id))]
[Description("Represents the current user in BCF 2.1")]
public class CurrentUserDataObject
{
    [JsonPropertyName("id")]
    [Description("The identifier of the user")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    [Description("The name of the user")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("uuid")]
    [Description("The UUID of the user")]
    [Required]
    public required string Uuid { get; init; }
}
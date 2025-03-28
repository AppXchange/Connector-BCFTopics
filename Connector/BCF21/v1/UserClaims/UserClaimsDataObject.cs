namespace Connector.BCF21.v1.UserClaims;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that represents the user claims in BCF 2.1
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents the user claims in BCF 2.1")]
public class UserClaimsDataObject
{
    [JsonPropertyName("id")]
    [Description("The identifier of the user")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("claims")]
    [Description("The claims associated with the user")]
    [Required]
    public required Dictionary<string, string> Claims { get; init; }

    [JsonPropertyName("access_token")]
    [Description("The access token for the user")]
    [Required]
    public required string AccessToken { get; init; }
}
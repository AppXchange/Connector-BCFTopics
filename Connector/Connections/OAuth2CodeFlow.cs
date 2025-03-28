using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

[ConnectionDefinition(title: "OAuth2CodeFlow", description: "Trimble ID OAuth2 Authorization Code Flow for Trimble Connect")]
public class OAuth2CodeFlow : OAuth2CodeFlowBase
{
    [ConnectionProperty(title: "Connection Environment", description: "The environment to connect to", isRequired: true, isSensitive: false)]
    public ConnectionEnvironmentOAuth2CodeFlow ConnectionEnvironment { get; set; } = ConnectionEnvironmentOAuth2CodeFlow.Unknown;

    [ConnectionProperty(title: "Redirect URI", description: "The redirect URI registered with Trimble Identity", isRequired: true, isSensitive: false)]
    public string RedirectUri { get; set; } = string.Empty;

    public string AuthBaseUrl
    {
        get
        {
            switch (ConnectionEnvironment)
            {
                case ConnectionEnvironmentOAuth2CodeFlow.Production:
                    return "https://id.trimble.com";
                case ConnectionEnvironmentOAuth2CodeFlow.Test:
                    return "https://id.trimble.com";
                default:
                    throw new Exception("No auth base url was set.");
            }
        }
    }

    public string ApiBaseUrl
    {
        get
        {
            switch (ConnectionEnvironment)
            {
                case ConnectionEnvironmentOAuth2CodeFlow.Production:
                    return "https://open11.connect.trimble.com";
                case ConnectionEnvironmentOAuth2CodeFlow.Test:
                    return "https://open11.stage.connect.trimble.com";
                default:
                    throw new Exception("No API base url was set.");
            }
        }
    }

    // The base URL property is used by the SDK for making API calls
    public string BaseUrl => ApiBaseUrl;

    // Auth endpoints for Trimble ID
    public string AuthorizationEndpoint => $"{AuthBaseUrl}/oauth/authorize";
    public string TokenEndpoint => $"{AuthBaseUrl}/oauth/token";

    public string GetAuthorizationUrl(string state)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "response_type", "code" },
            { "redirect_uri", RedirectUri },
            { "scope", Scope },
            { "state", state }
        };

        var queryString = string.Join("&", queryParams.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}"));
        return $"{AuthorizationEndpoint}?{queryString}";
    }

    public string GetBasicAuthHeader()
    {
        var credentials = $"{ClientId}:{ClientSecret}";
        var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
        var base64Credentials = Convert.ToBase64String(credentialsBytes);
        return $"Basic {base64Credentials}";
    }
}

public enum ConnectionEnvironmentOAuth2CodeFlow
{
    Unknown = 0,
    Production = 1,
    Test = 2
}
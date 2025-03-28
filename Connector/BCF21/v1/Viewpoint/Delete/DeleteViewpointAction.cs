namespace Connector.BCF21.v1.Viewpoint.Delete;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Deletes a viewpoint from a specific BCF 2.1 topic
/// </summary>
[Description("Deletes a viewpoint from a specific BCF 2.1 topic")]
public class DeleteViewpointAction : IStandardAction<DeleteViewpointActionInput, DeleteViewpointActionOutput>
{
    public DeleteViewpointActionInput ActionInput { get; set; } = new();
    public DeleteViewpointActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class DeleteViewpointActionInput
{
    // No input fields needed as parameters are passed through action parameters
}

public class DeleteViewpointActionOutput
{
    [JsonPropertyName("success")]
    [Description("Indicates if the deletion was successful")]
    public bool Success { get; init; }
}

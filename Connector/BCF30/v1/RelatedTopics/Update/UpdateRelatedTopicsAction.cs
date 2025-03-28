namespace Connector.BCF30.v1.RelatedTopics.Update;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object for updating related topics in BCF 3.0
/// </summary>
[Description("Updates the collection of related topics for a BCF 3.0 topic")]
public class UpdateRelatedTopicsAction : IStandardAction<UpdateRelatedTopicsActionInput, UpdateRelatedTopicsActionOutput>
{
    public UpdateRelatedTopicsActionInput ActionInput { get; set; } = null!;
    public UpdateRelatedTopicsActionOutput ActionOutput { get; set; } = null!;
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class RelatedTopicReference
{
    [JsonPropertyName("related_topic_guid")]
    [Description("The globally unique identifier of the related topic")]
    [Required]
    public required string RelatedTopicGuid { get; init; }
}

public class UpdateRelatedTopicsActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project containing the topic")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("topic_id")]
    [Description("The ID of the topic to update related topics for")]
    [Required]
    public required string TopicId { get; init; }

    [JsonPropertyName("related_topics")]
    [Description("The collection of related topics to set")]
    [Required]
    public required IEnumerable<RelatedTopicReference> RelatedTopics { get; init; }
}

public class UpdateRelatedTopicsActionOutput
{
    [JsonPropertyName("related_topics")]
    [Description("The updated collection of related topics")]
    [Required]
    public required IEnumerable<RelatedTopicReference> RelatedTopics { get; init; }
}

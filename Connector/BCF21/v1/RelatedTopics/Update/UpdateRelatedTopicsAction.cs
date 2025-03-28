namespace Connector.BCF21.v1.RelatedTopics.Update;

using Json.Schema.Generation;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action for updating related topics in BCF 2.1
/// </summary>
[Description("Updates the related topics for a specific topic in BCF 2.1")]
public class UpdateRelatedTopicsAction : IStandardAction<UpdateRelatedTopicsActionInput, IEnumerable<RelatedTopicsDataObject>>
{
    public UpdateRelatedTopicsActionInput ActionInput { get; set; } = new();
    public IEnumerable<RelatedTopicsDataObject> ActionOutput { get; set; } = new List<RelatedTopicsDataObject>();
    public StandardActionFailure ActionFailure { get; set; } = new();
    public bool CreateRtap => true;
}

public class UpdateRelatedTopicsActionInput
{
    [JsonPropertyName("project_id")]
    [Description("The ID of the project containing the topic")]
    [Required]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("topic_id")]
    [Description("The ID of the topic to update related topics for")]
    [Required]
    public string TopicId { get; set; } = string.Empty;

    [JsonPropertyName("related_topics")]
    [Description("The array of related topics to be updated")]
    [Required]
    public IEnumerable<RelatedTopicReference> RelatedTopics { get; set; } = new List<RelatedTopicReference>();
}

public class RelatedTopicReference
{
    [JsonPropertyName("related_topic_guid")]
    [Description("The GUID of the related topic")]
    [Required]
    public string RelatedTopicGuid { get; set; } = string.Empty;
}

namespace Connector.BCF21.v1;
using Connector.BCF21.v1.Comment.Create;
using Connector.BCF21.v1.Comment.Delete;
using Connector.BCF21.v1.Comment.Update;
using Connector.BCF21.v1.CommentsBatch.Create;
using Connector.BCF21.v1.CommentsBatch.Update;
using Connector.BCF21.v1.Document.Create;
using Connector.BCF21.v1.DocumentReferences.Create;
using Connector.BCF21.v1.DocumentReferences.Delete;
using Connector.BCF21.v1.DocumentReferences.Update;
using Connector.BCF21.v1.Files.Update;
using Connector.BCF21.v1.ProjectExtensions.Update;
using Connector.BCF21.v1.RelatedTopics.Update;
using Connector.BCF21.v1.Topic.Create;
using Connector.BCF21.v1.Topic.Delete;
using Connector.BCF21.v1.Topic.Update;
using Connector.BCF21.v1.TopicsBatch.Create;
using Connector.BCF21.v1.TopicsBatch.Update;
using Connector.BCF21.v1.Viewpoint.Create;
using Connector.BCF21.v1.Viewpoint.Delete;
using Json.Schema.Generation;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Configuration for the Action Processor for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("BCF21 V1 Action Processor Configuration")]
[Description("Configuration of the data object actions for the module.")]
public class BCF21V1ActionProcessorConfig
{
    // Action Handler configuration
    public DefaultActionHandlerConfig CreateCommentConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateCommentConfig { get; set; } = new();
    public DefaultActionHandlerConfig DeleteCommentConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateDocumentConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateDocumentReferencesConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateDocumentReferencesConfig { get; set; } = new();
    public DefaultActionHandlerConfig DeleteDocumentReferencesConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateFilesConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateProjectExtensionsConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateRelatedTopicsConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateTopicConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateTopicConfig { get; set; } = new();
    public DefaultActionHandlerConfig DeleteTopicConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateTopicsBatchConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateTopicsBatchConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateCommentsBatchConfig { get; set; } = new();
    public DefaultActionHandlerConfig UpdateCommentsBatchConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateViewpointConfig { get; set; } = new();
    public DefaultActionHandlerConfig DeleteViewpointConfig { get; set; } = new();
}
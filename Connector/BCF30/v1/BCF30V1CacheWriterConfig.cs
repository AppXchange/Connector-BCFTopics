namespace Connector.BCF30.v1;
using Connector.BCF30.v1.Comment;
using Connector.BCF30.v1.Comments;
using Connector.BCF30.v1.DefaultProjectExtensions;
using Connector.BCF30.v1.Document;
using Connector.BCF30.v1.DocumentReferences;
using Connector.BCF30.v1.Documents;
using Connector.BCF30.v1.Files;
using Connector.BCF30.v1.FilesInfo;
using Connector.BCF30.v1.Project;
using Connector.BCF30.v1.ProjectExtensions;
using Connector.BCF30.v1.Projects;
using Connector.BCF30.v1.RelatedTopics;
using Connector.BCF30.v1.Topic;
using Connector.BCF30.v1.Topics;
using Connector.BCF30.v1.Viewpoint;
using Connector.BCF30.v1.ViewpointColoring;
using Connector.BCF30.v1.Viewpoints;
using Connector.BCF30.v1.ViewpointSelection;
using Connector.BCF30.v1.ViewpointSnapshot;
using Connector.BCF30.v1.ViewpointVisibility;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("BCF30 V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class BCF30V1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig CommentsConfig { get; set; } = new();
    public CacheWriterObjectConfig CommentConfig { get; set; } = new();
    public CacheWriterObjectConfig DocumentsConfig { get; set; } = new();
    public CacheWriterObjectConfig DocumentConfig { get; set; } = new();
    public CacheWriterObjectConfig DocumentReferencesConfig { get; set; } = new();
    public CacheWriterObjectConfig FilesInfoConfig { get; set; } = new();
    public CacheWriterObjectConfig FilesConfig { get; set; } = new();
    public CacheWriterObjectConfig ProjectsConfig { get; set; } = new();
    public CacheWriterObjectConfig ProjectConfig { get; set; } = new();
    public CacheWriterObjectConfig ProjectExtensionsConfig { get; set; } = new();
    public CacheWriterObjectConfig DefaultProjectExtensionsConfig { get; set; } = new();
    public CacheWriterObjectConfig RelatedTopicsConfig { get; set; } = new();
    public CacheWriterObjectConfig TopicsConfig { get; set; } = new();
    public CacheWriterObjectConfig TopicConfig { get; set; } = new();
    public CacheWriterObjectConfig ViewpointsConfig { get; set; } = new();
    public CacheWriterObjectConfig ViewpointConfig { get; set; } = new();
    public CacheWriterObjectConfig ViewpointSnapshotConfig { get; set; } = new();
    public CacheWriterObjectConfig ViewpointSelectionConfig { get; set; } = new();
    public CacheWriterObjectConfig ViewpointColoringConfig { get; set; } = new();
    public CacheWriterObjectConfig ViewpointVisibilityConfig { get; set; } = new();
}
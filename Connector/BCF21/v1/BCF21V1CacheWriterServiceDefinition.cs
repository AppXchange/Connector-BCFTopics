namespace Connector.BCF21.v1;
using Connector.BCF21.v1.Comment;
using Connector.BCF21.v1.Comments;
using Connector.BCF21.v1.Comments.Models;
using Connector.BCF21.v1.CommentsBatch;
using Connector.BCF21.v1.CurrentUser;
using Connector.BCF21.v1.DefaultProjectExtensions;
using Connector.BCF21.v1.Document;
using Connector.BCF21.v1.DocumentReferences;
using Connector.BCF21.v1.Documents;
using Connector.BCF21.v1.Files;
using Connector.BCF21.v1.Project;
using Connector.BCF21.v1.ProjectExtensions;
using Connector.BCF21.v1.Projects;
using Connector.BCF21.v1.RelatedTopics;
using Connector.BCF21.v1.Topic;
using Connector.BCF21.v1.Topics;
using Connector.BCF21.v1.TopicsBatch;
using Connector.BCF21.v1.TopicsObjectsChanges;
using Connector.BCF21.v1.TopicsSyncingObjects;
using Connector.BCF21.v1.UserClaims;
using Connector.BCF21.v1.Versions;
using Connector.BCF21.v1.Viewpoint;
using Connector.BCF21.v1.ViewpointColoring;
using Connector.BCF21.v1.Viewpoints;
using Connector.BCF21.v1.ViewpointSelection;
using Connector.BCF21.v1.ViewpointSnapshot;
using Connector.BCF21.v1.ViewpointVisibility;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class BCF21V1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<BCF21V1CacheWriterConfig>
{
    public override string ModuleId => "bcf21-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<BCF21V1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<BCF21V1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<BCF21V1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<BCF21V1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<BCF21V1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<CommentsDataReader>();
        serviceCollection.AddSingleton<CommentDataReader>();
        serviceCollection.AddSingleton<DocumentsDataReader>();
        serviceCollection.AddSingleton<DocumentDataReader>();
        serviceCollection.AddSingleton<DocumentReferencesDataReader>();
        serviceCollection.AddSingleton<FilesDataReader>();
        serviceCollection.AddSingleton<ProjectsDataReader>();
        serviceCollection.AddSingleton<ProjectDataReader>();
        serviceCollection.AddSingleton<ProjectExtensionsDataReader>();
        serviceCollection.AddSingleton<DefaultProjectExtensionsDataReader>();
        serviceCollection.AddSingleton<RelatedTopicsDataReader>();
        serviceCollection.AddSingleton<TopicsDataReader>();
        serviceCollection.AddSingleton<TopicDataReader>();
        serviceCollection.AddSingleton<TopicsSyncingObjectsDataReader>();
        serviceCollection.AddSingleton<TopicsObjectsChangesDataReader>();
        serviceCollection.AddSingleton<TopicsBatchDataReader>();
        serviceCollection.AddSingleton<CommentsBatchDataReader>();
        serviceCollection.AddSingleton<CurrentUserDataReader>();
        serviceCollection.AddSingleton<UserClaimsDataReader>();
        serviceCollection.AddSingleton<VersionsDataReader>();
        serviceCollection.AddSingleton<ViewpointsDataReader>();
        serviceCollection.AddSingleton<ViewpointDataReader>();
        serviceCollection.AddSingleton<ViewpointSnapshotDataReader>();
        serviceCollection.AddSingleton<ViewpointSelectionDataReader>();
        serviceCollection.AddSingleton<ViewpointColoringDataReader>();
        serviceCollection.AddSingleton<ViewpointVisibilityDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<CommentsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<CommentDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<DocumentsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<DocumentDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<DocumentReferencesDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<FilesDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ProjectsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ProjectDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ProjectExtensionsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<DefaultProjectExtensionsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<RelatedTopicsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TopicsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TopicDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TopicsSyncingObjectsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TopicsObjectsChangesDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TopicsBatchDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<CommentsBatchDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<CurrentUserDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<UserClaimsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<VersionsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ViewpointsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ViewpointDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ViewpointSnapshotDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ViewpointSelectionDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ViewpointColoringDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ViewpointVisibilityDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, BCF21V1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<CommentsDataReader, CommentsDataObject>(ModuleId, config.CommentsConfig, dataReaderSettings);
        service.RegisterDataReader<CommentDataReader, CommentDataObject>(ModuleId, config.CommentConfig, dataReaderSettings);
        service.RegisterDataReader<DocumentsDataReader, DocumentsDataObject>(ModuleId, config.DocumentsConfig, dataReaderSettings);
        service.RegisterDataReader<DocumentDataReader, DocumentDataObject>(ModuleId, config.DocumentConfig, dataReaderSettings);
        service.RegisterDataReader<DocumentReferencesDataReader, DocumentReferencesDataObject>(ModuleId, config.DocumentReferencesConfig, dataReaderSettings);
        service.RegisterDataReader<FilesDataReader, FilesDataObject>(ModuleId, config.FilesConfig, dataReaderSettings);
        service.RegisterDataReader<ProjectsDataReader, ProjectsDataObject>(ModuleId, config.ProjectsConfig, dataReaderSettings);
        service.RegisterDataReader<ProjectDataReader, ProjectDataObject>(ModuleId, config.ProjectConfig, dataReaderSettings);
        service.RegisterDataReader<ProjectExtensionsDataReader, ProjectExtensionsDataObject>(ModuleId, config.ProjectExtensionsConfig, dataReaderSettings);
        service.RegisterDataReader<DefaultProjectExtensionsDataReader, DefaultProjectExtensionsDataObject>(ModuleId, config.DefaultProjectExtensionsConfig, dataReaderSettings);
        service.RegisterDataReader<RelatedTopicsDataReader, RelatedTopicsDataObject>(ModuleId, config.RelatedTopicsConfig, dataReaderSettings);
        service.RegisterDataReader<TopicsDataReader, TopicsDataObject>(ModuleId, config.TopicsConfig, dataReaderSettings);
        service.RegisterDataReader<TopicDataReader, TopicDataObject>(ModuleId, config.TopicConfig, dataReaderSettings);
        service.RegisterDataReader<TopicsSyncingObjectsDataReader, TopicsSyncingObjectsDataObject>(ModuleId, config.TopicsSyncingObjectsConfig, dataReaderSettings);
        service.RegisterDataReader<TopicsObjectsChangesDataReader, TopicsObjectsChangesDataObject>(ModuleId, config.TopicsObjectsChangesConfig, dataReaderSettings);
        service.RegisterDataReader<TopicsBatchDataReader, TopicsBatchDataObject>(ModuleId, config.TopicsBatchConfig, dataReaderSettings);
        service.RegisterDataReader<CommentsBatchDataReader, CommentsBatchDataObject>(ModuleId, config.CommentsBatchConfig, dataReaderSettings);
        service.RegisterDataReader<CurrentUserDataReader, CurrentUserDataObject>(ModuleId, config.CurrentUserConfig, dataReaderSettings);
        service.RegisterDataReader<UserClaimsDataReader, UserClaimsDataObject>(ModuleId, config.UserClaimsConfig, dataReaderSettings);
        service.RegisterDataReader<VersionsDataReader, VersionsDataObject>(ModuleId, config.VersionsConfig, dataReaderSettings);
        service.RegisterDataReader<ViewpointsDataReader, ViewpointsDataObject>(ModuleId, config.ViewpointsConfig, dataReaderSettings);
        service.RegisterDataReader<ViewpointDataReader, ViewpointDataObject>(ModuleId, config.ViewpointConfig, dataReaderSettings);
        service.RegisterDataReader<ViewpointSnapshotDataReader, ViewpointSnapshotDataObject>(ModuleId, config.ViewpointSnapshotConfig, dataReaderSettings);
        service.RegisterDataReader<ViewpointSelectionDataReader, ViewpointSelectionDataObject>(ModuleId, config.ViewpointSelectionConfig, dataReaderSettings);
        service.RegisterDataReader<ViewpointColoringDataReader, ViewpointColoringDataObject>(ModuleId, config.ViewpointColoringConfig, dataReaderSettings);
        service.RegisterDataReader<ViewpointVisibilityDataReader, ViewpointVisibilityDataObject>(ModuleId, config.ViewpointVisibilityConfig, dataReaderSettings);
    }
}
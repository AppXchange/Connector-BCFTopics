namespace Connector.BCF30.v1;
using Connector.BCF30.v1.Comment;
using Connector.BCF30.v1.Comment.Create;
using Connector.BCF30.v1.Comment.Delete;
using Connector.BCF30.v1.Comment.Update;
using Connector.BCF30.v1.Document;
using Connector.BCF30.v1.Document.Create;
using Connector.BCF30.v1.DocumentReferences;
using Connector.BCF30.v1.DocumentReferences.Create;
using Connector.BCF30.v1.DocumentReferences.Delete;
using Connector.BCF30.v1.DocumentReferences.Update;
using Connector.BCF30.v1.Files;
using Connector.BCF30.v1.Files.Update;
using Connector.BCF30.v1.ProjectExtensions;
using Connector.BCF30.v1.ProjectExtensions.Update;
using Connector.BCF30.v1.RelatedTopics;
using Connector.BCF30.v1.RelatedTopics.Update;
using Connector.BCF30.v1.Topic;
using Connector.BCF30.v1.Topic.Create;
using Connector.BCF30.v1.Topic.Delete;
using Connector.BCF30.v1.Topic.Update;
using Connector.BCF30.v1.Viewpoint;
using Connector.BCF30.v1.Viewpoint.Create;
using Connector.BCF30.v1.Viewpoint.Delete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class BCF30V1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<BCF30V1ActionProcessorConfig>
{
    public override string ModuleId => "bcf30-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<BCF30V1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        var serviceConfig = JsonSerializer.Deserialize<BCF30V1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<BCF30V1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<BCF30V1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<BCF30V1ActionProcessorConfig>>(this);
        // Register Action Handlers as scoped dependencies
        serviceCollection.AddScoped<CreateCommentHandler>();
        serviceCollection.AddScoped<UpdateCommentHandler>();
        serviceCollection.AddScoped<DeleteCommentHandler>();
        serviceCollection.AddScoped<CreateDocumentHandler>();
        serviceCollection.AddScoped<CreateDocumentReferencesHandler>();
        serviceCollection.AddScoped<UpdateDocumentReferencesHandler>();
        serviceCollection.AddScoped<DeleteDocumentReferencesHandler>();
        serviceCollection.AddScoped<UpdateFilesHandler>();
        serviceCollection.AddScoped<UpdateProjectExtensionsHandler>();
        serviceCollection.AddScoped<UpdateRelatedTopicsHandler>();
        serviceCollection.AddScoped<CreateTopicHandler>();
        serviceCollection.AddScoped<UpdateTopicHandler>();
        serviceCollection.AddScoped<DeleteTopicHandler>();
        serviceCollection.AddScoped<CreateViewpointHandler>();
        serviceCollection.AddScoped<DeleteViewpointHandler>();
    }

    public override void ConfigureService(IActionHandlerService service, BCF30V1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
        service.RegisterHandlerForDataObjectAction<CreateCommentHandler, CommentDataObject>(ModuleId, "comment", "create", config.CreateCommentConfig);
        service.RegisterHandlerForDataObjectAction<UpdateCommentHandler, CommentDataObject>(ModuleId, "comment", "update", config.UpdateCommentConfig);
        service.RegisterHandlerForDataObjectAction<DeleteCommentHandler, CommentDataObject>(ModuleId, "comment", "delete", config.DeleteCommentConfig);
        service.RegisterHandlerForDataObjectAction<CreateDocumentHandler, DocumentDataObject>(ModuleId, "document", "create", config.CreateDocumentConfig);
        service.RegisterHandlerForDataObjectAction<CreateDocumentReferencesHandler, DocumentReferencesDataObject>(ModuleId, "document-references", "create", config.CreateDocumentReferencesConfig);
        service.RegisterHandlerForDataObjectAction<UpdateDocumentReferencesHandler, DocumentReferencesDataObject>(ModuleId, "document-references", "update", config.UpdateDocumentReferencesConfig);
        service.RegisterHandlerForDataObjectAction<DeleteDocumentReferencesHandler, DocumentReferencesDataObject>(ModuleId, "document-references", "delete", config.DeleteDocumentReferencesConfig);
        service.RegisterHandlerForDataObjectAction<UpdateFilesHandler, FilesDataObject>(ModuleId, "files", "update", config.UpdateFilesConfig);
        service.RegisterHandlerForDataObjectAction<UpdateProjectExtensionsHandler, ProjectExtensionsDataObject>(ModuleId, "project-extensions", "update", config.UpdateProjectExtensionsConfig);
        service.RegisterHandlerForDataObjectAction<UpdateRelatedTopicsHandler, RelatedTopicsDataObject>(ModuleId, "related-topics", "update", config.UpdateRelatedTopicsConfig);
        service.RegisterHandlerForDataObjectAction<CreateTopicHandler, TopicDataObject>(ModuleId, "topic", "create", config.CreateTopicConfig);
        service.RegisterHandlerForDataObjectAction<UpdateTopicHandler, TopicDataObject>(ModuleId, "topic", "update", config.UpdateTopicConfig);
        service.RegisterHandlerForDataObjectAction<DeleteTopicHandler, TopicDataObject>(ModuleId, "topic", "delete", config.DeleteTopicConfig);
        service.RegisterHandlerForDataObjectAction<CreateViewpointHandler, ViewpointDataObject>(ModuleId, "viewpoint", "create", config.CreateViewpointConfig);
        service.RegisterHandlerForDataObjectAction<DeleteViewpointHandler, ViewpointDataObject>(ModuleId, "viewpoint", "delete", config.DeleteViewpointConfig);
    }
}
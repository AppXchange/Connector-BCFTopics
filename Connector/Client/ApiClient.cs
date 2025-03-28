using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Connector.BCF30.v1.Comments.Models;
using Connector.BCF21.v1.Comments.Models;
using Connector.BCF30.v1.Comment.Models;
using Connector.BCF30.v1.Comment.Create;
using Connector.BCF30.v1.Comment.Update;
using Connector.BCF30.v1.Documents.Models;
using Connector.BCF30.v1.Document.Models;
using Connector.BCF30.v1.Document.Create;
using Connector.BCF30.v1.DocumentReferences.Models;
using Connector.BCF30.v1.DocumentReferences.Create;
using Connector.BCF30.v1.DocumentReferences.Update;
using Connector.BCF30.v1.FilesInfo;
using Connector.BCF30.v1.Project;
using Connector.BCF30.v1.ProjectExtensions;
using Connector.BCF30.v1.ProjectExtensions.Update;
using Connector.BCF30.v1.DefaultProjectExtensions;
using Connector.BCF30.v1.RelatedTopics;
using Connector.BCF30.v1.RelatedTopics.Update;
using Connector.BCF30.v1.Topics;
using Connector.BCF30.v1.Topic;
using Connector.BCF30.v1.Topic.Create;
using Connector.BCF30.v1.Topic.Update;
using Connector.BCF30.v1.Viewpoints;
using Connector.BCF30.v1.Viewpoint;
using Connector.BCF30.v1.ViewpointColoring;
using Connector.BCF30.v1.ViewpointSelection;
using Connector.BCF30.v1.ViewpointSnapshot;
using Connector.BCF30.v1.ViewpointVisibility;
using Connector.BCF21.v1.Comment.Create;
using Connector.BCF21.v1.TopicsBatch;
using Connector.BCF21.v1.TopicsBatch.Create;
using Connector.BCF21.v1.CommentsBatch;
using Connector.BCF21.v1.CommentsBatch.Create;
using Connector.BCF21.v1.CommentsBatch.Update;
using System.Text.Json;
using System;
using System.IO;
using Connector.BCF21.v1.TopicsBatch.Update;
using Connector.BCF21.v1.CurrentUser;
using Connector.BCF21.v1.UserClaims;
using Connector.BCF21.v1.Viewpoint.Create;

namespace Connector.Client;

/// <summary>
/// A client for interfacing with the API via the HTTP protocol.
/// </summary>
public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public ApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new System.Uri(baseUrl);
    }

    // Example of a paginated response.
    public async Task<ApiResponse<PaginatedResponse<T>>> GetRecords<T>(string relativeUrl, int page, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{relativeUrl}?page={page}", cancellationToken: cancellationToken).ConfigureAwait(false);
        return new ApiResponse<PaginatedResponse<T>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<PaginatedResponse<T>>(cancellationToken: cancellationToken) : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> GetNoContent(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("no-content", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> TestConnection(CancellationToken cancellationToken = default)
    {
        // The purpose of this method is to validate that successful and authorized requests can be made to the API.
        // In this example, we are using the GET "oauth/me" endpoint.
        // Choose any endpoint that you consider suitable for testing the connection with the API.

        var response = await _httpClient
            .GetAsync($"oauth/me", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers
        };
    }

    #region BCF 3.0 Endpoints

    /// <summary>
    /// Gets all comments for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.Comments.Models.CommentsDataObject>>> GetBcf30Comments(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/comments", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF30.v1.Comments.Models.CommentsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF30.v1.Comments.Models.CommentsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets a single comment for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.Comment.Models.CommentDataObject>> GetBcf30Comment(
        string projectId,
        string topicId,
        string commentId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/comments/{commentId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<CommentDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<CommentDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Creates a new comment for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.Comment.Create.CreateCommentActionOutput>> CreateBcf30Comment(
        string projectId,
        string topicId,
        string comment,
        string? guid = null,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            comment,
            guid
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}/comments",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.Comment.Create.CreateCommentActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.Comment.Create.CreateCommentActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Deletes a comment from a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse> DeleteBcf30Comment(
        string projectId,
        string topicId,
        string commentId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/comments/{commentId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Updates a comment for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.Comment.Update.UpdateCommentActionOutput>> UpdateBcf30Comment(
        string projectId,
        string topicId,
        string commentId,
        string comment,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            comment
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}/comments/{commentId}",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<UpdateCommentActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<UpdateCommentActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all documents for a specific BCF 3.0 project
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.Documents.Models.DocumentsDataObject>>> GetBcf30Documents(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/documents", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<DocumentsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<DocumentsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets a single document from a BCF 3.0 project
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.Document.Models.DocumentDataObject>> GetBcf30Document(
        string projectId,
        string documentId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/documents/{documentId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var content = response.Content != null && response.IsSuccessStatusCode ? 
            await response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken) : 
            null;

        // Get the filename from the Content-Disposition header if available
        var filename = response.Content?.Headers?.ContentDisposition?.FileName ?? documentId;

        return new ApiResponse<DocumentDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode && content != null ? 
                new DocumentDataObject 
                { 
                    Guid = documentId,
                    Filename = filename,
                    Content = content
                } : 
                default,
            RawResult = response.Content != null ? 
                await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken) : 
                null
        };
    }

    /// <summary>
    /// Creates a new document in a BCF 3.0 project
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.Document.Create.CreateDocumentActionOutput>> CreateBcf30Document(
        string projectId,
        string filename,
        byte[] content,
        CancellationToken cancellationToken = default)
    {
        var byteContent = new ByteArrayContent(content);
        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        byteContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        {
            FileName = filename
        };

        var response = await _httpClient
            .PostAsync($"bcf/3.0/projects/{projectId}/documents", byteContent, cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<CreateDocumentActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<CreateDocumentActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all document references for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.DocumentReferences.Models.DocumentReferencesDataObject>>> GetBcf30DocumentReferences(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/document_references", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<DocumentReferencesDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<DocumentReferencesDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Creates a new document reference in a BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.DocumentReferences.Create.CreateDocumentReferencesActionOutput>> CreateBcf30DocumentReference(
        string projectId,
        string topicId,
        string? url,
        string? documentGuid,
        string description,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            url,
            document_guid = documentGuid,
            description
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}/document_references",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<CreateDocumentReferencesActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<CreateDocumentReferencesActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Updates a document reference in a BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.DocumentReferences.Update.UpdateDocumentReferencesActionOutput>> UpdateBcf30DocumentReference(
        string projectId,
        string topicId,
        string documentReferenceId,
        string? url,
        string? documentGuid,
        string description,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            url,
            document_guid = documentGuid,
            description
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}/document_references/{documentReferenceId}",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<UpdateDocumentReferencesActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<UpdateDocumentReferencesActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Deletes a document reference from a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse> DeleteBcf30DocumentReference(
        string projectId,
        string topicId,
        string documentReferenceId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/document_references/{documentReferenceId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all files for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.Files.FilesDataObject>>> GetBcf30Files(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/files", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF30.v1.Files.FilesDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF30.v1.Files.FilesDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Updates files for a specific BCF 3.0 topic
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.Files.FilesDataObject>>> UpdateBcf30Files(
        string projectId,
        string topicId,
        IEnumerable<BCF30.v1.Files.Update.FileReference> files,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}/files",
            files,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF30.v1.Files.FilesDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF30.v1.Files.FilesDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets files information for a specific BCF 3.0 project
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.FilesInfo.FilesInfoDataObject>>> GetBcf30FilesInformation(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/files_information", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<FilesInfoDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<FilesInfoDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all projects for BCF 3.0
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF30.v1.Projects.ProjectsDataObject>>> GetBcf30Projects(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("bcf/3.0/projects", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF30.v1.Projects.ProjectsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF30.v1.Projects.ProjectsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets a specific project for BCF 3.0
    /// </summary>
    public async Task<ApiResponse<BCF30.v1.Project.ProjectDataObject>> GetBcf30Project(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<ProjectDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<ProjectDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<ProjectExtensionsDataObject>> GetBcf30ProjectExtensions(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/extensions", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<ProjectExtensionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<ProjectExtensionsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.ProjectExtensions.ProjectExtensionsDataObject>> UpdateBcf30ProjectExtensions(
        string projectId,
        IEnumerable<BCF30.v1.ProjectExtensions.Update.ExtensionValue> priorityFull,
        string topicServerAssignedIdPrefix,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            priority_full = priorityFull,
            topic_server_assigned_id_prefix = topicServerAssignedIdPrefix
        };

        var response = await _httpClient
            .PutAsync(
                $"bcf/3.0/projects/{projectId}/extensions",
                JsonContent.Create(content),
                cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<ProjectExtensionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<ProjectExtensionsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.DefaultProjectExtensions.DefaultProjectExtensionsDataObject>> GetBcf30DefaultProjectExtensions(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/defaultextensions", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<DefaultProjectExtensionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<DefaultProjectExtensionsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all related topics for a specific BCF 3.0 topic
    /// </summary>
    /// <param name="projectId">The ID of the project containing the topic</param>
    /// <param name="topicId">The ID of the topic to get related topics for</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>An ApiResponse containing the collection of related topics</returns>
    public async Task<ApiResponse<IEnumerable<RelatedTopicsDataObject>>> GetBcf30RelatedTopics(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/related_topics", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<RelatedTopicsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<RelatedTopicsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF30.v1.RelatedTopics.Update.RelatedTopicReference>>> UpdateBcf30RelatedTopics(
        string projectId,
        string topicId,
        IEnumerable<RelatedTopicReference> relatedTopics,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsync(
                $"bcf/3.0/projects/{projectId}/topics/{topicId}/related_topics",
                JsonContent.Create(relatedTopics),
                cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<RelatedTopicReference>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<RelatedTopicReference>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF30.v1.Topics.TopicsDataObject>>> GetBcf30Topics(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF30.v1.Topics.TopicsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF30.v1.Topics.TopicsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.Topic.TopicDataObject>> GetBcf30Topic(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.Topic.TopicDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.Topic.TopicDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.Topic.Create.CreateTopicActionOutput>> CreateBcf30Topic(
        string projectId,
        string topicType,
        string topicStatus,
        string title,
        string priority,
        IEnumerable<string> labels,
        string? assignedTo = null,
        BCF30.v1.Topic.Update.BimSnippetInput? bimSnippet = null,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            topic_type = topicType,
            topic_status = topicStatus,
            title,
            priority,
            labels,
            assigned_to = assignedTo,
            bim_snippet = bimSnippet
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.Topic.Create.CreateTopicActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.Topic.Create.CreateTopicActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<UpdateTopicActionOutput>> UpdateBcf30Topic(
        string projectId,
        string topicId,
        string topicType,
        string topicStatus,
        string title,
        string priority,
        IEnumerable<string> labels,
        string? assignedTo = null,
        Connector.BCF30.v1.Topic.Update.BimSnippetInput? bimSnippet = null,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            topic_type = topicType,
            topic_status = topicStatus,
            title,
            priority,
            labels,
            assigned_to = assignedTo,
            bim_snippet = bimSnippet
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<UpdateTopicActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<UpdateTopicActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> DeleteBcf30Topic(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF30.v1.Viewpoints.ViewpointsDataObject>>> GetBcf30Viewpoints(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<IEnumerable<BCF30.v1.Viewpoints.ViewpointsDataObject>>(content);
            
            return new ApiResponse<IEnumerable<BCF30.v1.Viewpoints.ViewpointsDataObject>>
            {
                IsSuccessful = true,
                StatusCode = (int)response.StatusCode,
                Data = data
            };
        }

        var rawResult = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return new ApiResponse<IEnumerable<BCF30.v1.Viewpoints.ViewpointsDataObject>>
        {
            IsSuccessful = false,
            StatusCode = (int)response.StatusCode,
            RawResult = rawResult
        };
    }

    public async Task<ApiResponse<BCF30.v1.Viewpoint.ViewpointDataObject>> GetBcf30Viewpoint(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.Viewpoint.ViewpointDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.Viewpoint.ViewpointDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.Viewpoint.Create.CreateViewpointActionOutput>> CreateBcf30Viewpoint(
        string projectId,
        string topicId,
        int index,
        BCF30.v1.Viewpoint.PerspectiveCamera perspectiveCamera,
        IEnumerable<BCF30.v1.Viewpoint.Line>? lines = null,
        IEnumerable<BCF30.v1.Viewpoint.ClippingPlane>? clippingPlanes = null,
        BCF30.v1.Viewpoint.Create.SnapshotInput? snapshot = null,
        BCF30.v1.Viewpoint.Create.Components? components = null,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            index,
            perspective_camera = perspectiveCamera,
            lines,
            clipping_planes = clippingPlanes,
            snapshot,
            components
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.Viewpoint.Create.CreateViewpointActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.Viewpoint.Create.CreateViewpointActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> DeleteBcf30Viewpoint(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .DeleteAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.ViewpointColoring.ViewpointColoringDataObject>> GetBcf30ViewpointColoring(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/coloring", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.ViewpointColoring.ViewpointColoringDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.ViewpointColoring.ViewpointColoringDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF30.v1.ViewpointSelection.ViewpointSelectionDataObject>> GetBcf30ViewpointSelection(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/selection", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.ViewpointSelection.ViewpointSelectionDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.ViewpointSelection.ViewpointSelectionDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }


    public async Task<ApiResponse<BCF30.v1.ViewpointSnapshot.ViewpointSnapshotDataObject>> GetBcf30ViewpointSnapshot(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/snapshot", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var snapshotData = response.Content != null && response.IsSuccessStatusCode ? 
            await response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken) : 
            null;

        // Get the content type to determine snapshot type
        var snapshotType = response.Content?.Headers?.ContentType?.MediaType?.Split('/').LastOrDefault()?.ToLowerInvariant() ?? "png";

        return new ApiResponse<BCF30.v1.ViewpointSnapshot.ViewpointSnapshotDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode && snapshotData != null ? 
                new BCF30.v1.ViewpointSnapshot.ViewpointSnapshotDataObject 
                { 
                    ViewpointId = viewpointId,
                    SnapshotData = snapshotData,
                    SnapshotType = snapshotType
                } : 
                default,
            RawResult = response.Content != null ? 
                await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken) : 
                null
        };
    }

    public async Task<ApiResponse<BCF30.v1.ViewpointVisibility.ViewpointVisibilityDataObject>> GetBcf30ViewpointVisibility(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/3.0/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/visibility", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF30.v1.ViewpointVisibility.ViewpointVisibilityDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF30.v1.ViewpointVisibility.ViewpointVisibilityDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = response.Content != null ? 
                await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken) : 
                null
        };
    }

    #endregion

    #region BCF 2.1 Endpoints

    public async Task<ApiResponse<IEnumerable<BCF21.v1.Versions.VersionsDataObject>>> GetBcfVersions(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("bcf/versions", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.Versions.VersionsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.Versions.VersionsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all topics for a specific BCF 2.1 project
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF21.v1.Topics.TopicsDataObject>>> GetBcf21Topics(
        string projectId,
        int top = 500,
        string? skipToken = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/topics";
        var queryParams = new List<string>();

        if (top != 500)
        {
            queryParams.Add($"top={top}");
        }

        if (!string.IsNullOrEmpty(skipToken))
        {
            queryParams.Add($"skiptoken={skipToken}");
        }

        if (queryParams.Any())
        {
            url += "?" + string.Join("&", queryParams);
        }

        var response = await _httpClient
            .GetAsync(url, cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.Topics.TopicsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.Topics.TopicsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    /// <summary>
    /// Gets all comments for a specific BCF 2.1 topic
    /// </summary>
    public async Task<ApiResponse<IEnumerable<BCF21.v1.Comments.Models.CommentsDataObject>>> GetBcf21Comments(
        string projectId,
        string topicId,
        int? top = 500,
        string? skipToken = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/topics/{topicId}/comments";
        if (top.HasValue)
        {
            url += $"?top={top}";
        }
        if (!string.IsNullOrEmpty(skipToken))
        {
            url += top.HasValue ? "&" : "?";
            url += $"skiptoken={skipToken}";
        }

        var response = await _httpClient
            .GetAsync(url, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.Comments.Models.CommentsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.Comments.Models.CommentsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Comment.CommentDataObject>> GetBcf21Comment(
        string projectId,
        string topicId,
        string commentId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/topics/{topicId}/comments/{commentId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Comment.CommentDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Comment.CommentDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = response.Content != null ? 
                await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken) : 
                null
        };
    }

    public async Task<ApiResponse<BCF21.v1.Comment.Create.CreateCommentActionOutput>> CreateBcf21Comment(
        string projectId,
        string topicId,
        BCF21.v1.Comment.Create.CreateCommentActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/comments",
            input,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Comment.Create.CreateCommentActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Comment.Create.CreateCommentActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Comment.Update.UpdateCommentActionOutput>> UpdateBcf21Comment(
        string projectId,
        string topicId,
        string commentId,
        BCF21.v1.Comment.Update.UpdateCommentActionInput input,
        CancellationToken cancellationToken = default)
    {
        var content = new
        {
            comment = input.Comment,
            viewpoint_guid = input.ViewpointGuid,
            reply_to_comment_guid = input.ReplyToCommentGuid
        };

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/comments/{commentId}",
            content,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Comment.Update.UpdateCommentActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Comment.Update.UpdateCommentActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> DeleteBcf21Comment(
        string projectId,
        string topicId,
        string commentId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync($"bcf/2.1/projects/{projectId}/topics/{topicId}/comments/{commentId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.Documents.DocumentsDataObject>>> GetBcf21Documents(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/documents", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.Documents.DocumentsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.Documents.DocumentsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Document.DocumentDataObject>> GetBcf21Document(
        string projectId,
        string documentId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/documents/{documentId}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var content = response.Content != null && response.IsSuccessStatusCode ? 
            await response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken) : 
            null;

        // Get the filename from the Content-Disposition header if available
        var filename = response.Content?.Headers?.ContentDisposition?.FileName ?? documentId;

        return new ApiResponse<BCF21.v1.Document.DocumentDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode && content != null ? 
                new BCF21.v1.Document.DocumentDataObject 
                { 
                    Guid = documentId,
                    Filename = filename,
                    Content = content
                } : 
                default,
            RawResult = response.Content != null ? 
                await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken) : 
                null
        };
    }

    public async Task<ApiResponse<BCF21.v1.Document.Create.CreateDocumentActionOutput>> CreateBcf21Document(
        string projectId,
        string filename,
        byte[] content,
        CancellationToken cancellationToken = default)
    {
        var byteContent = new ByteArrayContent(content);
        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        byteContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        {
            FileName = filename
        };

        var response = await _httpClient
            .PostAsync($"bcf/2.1/projects/{projectId}/documents", byteContent, cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Document.Create.CreateDocumentActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Document.Create.CreateDocumentActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.DocumentReferences.DocumentReferencesDataObject>>> GetBcf21DocumentReferences(
        string projectId,
        string topicId,
        int top = 500,
        string? skipToken = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/topics/{topicId}/document_references";
        var queryParams = new List<string>();

        if (top != 500)
        {
            queryParams.Add($"top={top}");
        }

        if (!string.IsNullOrEmpty(skipToken))
        {
            queryParams.Add($"skiptoken={skipToken}");
        }

        if (queryParams.Any())
        {
            url += "?" + string.Join("&", queryParams);
        }

        var response = await _httpClient
            .GetAsync(url, cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.DocumentReferences.DocumentReferencesDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.DocumentReferences.DocumentReferencesDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.DocumentReferences.Create.CreateDocumentReferencesActionOutput>> CreateBcf21DocumentReference(
        string projectId,
        string topicId,
        object request,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/document_references",
            request,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.DocumentReferences.Create.CreateDocumentReferencesActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.DocumentReferences.Create.CreateDocumentReferencesActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.DocumentReferences.Update.UpdateDocumentReferencesActionOutput>> UpdateBcf21DocumentReference(
        string projectId,
        string topicId,
        string documentReferenceId,
        object request,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/document_references/{documentReferenceId}",
            request,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.DocumentReferences.Update.UpdateDocumentReferencesActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.DocumentReferences.Update.UpdateDocumentReferencesActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> DeleteBcf21DocumentReference(
        string projectId,
        string topicId,
        string documentReferenceId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync($"bcf/2.1/projects/{projectId}/topics/{topicId}/document_references/{documentReferenceId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.Files.FilesDataObject>>> GetBcf21TopicFiles(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/topics/{topicId}/files", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.Files.FilesDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.Files.FilesDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<List<BCF21.v1.Files.Update.FileReference>>> UpdateBcf21TopicFiles(
        string projectId,
        string topicId,
        List<BCF21.v1.Files.Update.FileReference> files,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/files",
            files,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<List<BCF21.v1.Files.Update.FileReference>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<List<BCF21.v1.Files.Update.FileReference>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.Projects.ProjectsDataObject>>> GetBcf21Projects(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("bcf/2.1/projects", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.Projects.ProjectsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.Projects.ProjectsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Project.ProjectDataObject>> GetBcf21Project(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Project.ProjectDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Project.ProjectDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.ProjectExtensions.ProjectExtensionsDataObject>> GetBcf21ProjectExtensions(
        string projectId,
        bool includeUsers = true,
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/extensions";
        if (!includeUsers)
        {
            url += "?includeUsers=false";
        }

        var response = await _httpClient
            .GetAsync(url, cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.ProjectExtensions.ProjectExtensionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.ProjectExtensions.ProjectExtensionsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.ProjectExtensions.ProjectExtensionsDataObject>> UpdateBcf21ProjectExtensions(
        string projectId,
        object request,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/extensions",
            request,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.ProjectExtensions.ProjectExtensionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.ProjectExtensions.ProjectExtensionsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.DefaultProjectExtensions.DefaultProjectExtensionsDataObject>> GetBcf21DefaultProjectExtensions(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/defaultextensions", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.DefaultProjectExtensions.DefaultProjectExtensionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.DefaultProjectExtensions.DefaultProjectExtensionsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.RelatedTopics.RelatedTopicsDataObject>>> GetBcf21RelatedTopics(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/topics/{topicId}/related_topics", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.RelatedTopics.RelatedTopicsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.RelatedTopics.RelatedTopicsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.RelatedTopics.RelatedTopicsDataObject>>> UpdateBcf21RelatedTopics(
        string projectId,
        string topicId,
        IEnumerable<BCF21.v1.RelatedTopics.Update.RelatedTopicReference> relatedTopics,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/related_topics",
            relatedTopics,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<IEnumerable<BCF21.v1.RelatedTopics.RelatedTopicsDataObject>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<IEnumerable<BCF21.v1.RelatedTopics.RelatedTopicsDataObject>>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Topic.TopicDataObject>> GetBcf21Topic(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"bcf/2.1/projects/{projectId}/topics/{topicId}", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Topic.TopicDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Topic.TopicDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Topic.Create.CreateTopicActionOutput>> CreateBcf21Topic(
        string projectId,
        BCF21.v1.Topic.Create.CreateTopicActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics",
            input,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Topic.Create.CreateTopicActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Topic.Create.CreateTopicActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Topic.Update.UpdateTopicActionOutput>> UpdateBcf21Topic(
        string projectId,
        string topicId,
        BCF21.v1.Topic.Update.UpdateTopicActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PutAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}",
            input,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Topic.Update.UpdateTopicActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.Topic.Update.UpdateTopicActionOutput>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.Topic.Delete.DeleteTopicActionOutput>> DeleteBcf21Topic(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}",
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.Topic.Delete.DeleteTopicActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                new BCF21.v1.Topic.Delete.DeleteTopicActionOutput { Success = true } : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.TopicsSyncingObjects.TopicsSyncingObjectsDataObject>> GetBcf21SyncingObjects(
        string projectId,
        string type,
        bool fetchAll = false,
        int pageSize = 100,
        string? skipToken = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/objects";
        var queryParams = new List<string>
        {
            $"type={type}",
            $"fetchAll={fetchAll.ToString().ToLowerInvariant()}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrEmpty(skipToken))
        {
            queryParams.Add($"skiptoken={skipToken}");
        }

        url += "?" + string.Join("&", queryParams);

        var response = await _httpClient
            .GetAsync(url, cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.TopicsSyncingObjects.TopicsSyncingObjectsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.TopicsSyncingObjects.TopicsSyncingObjectsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.TopicsObjectsChanges.TopicsObjectsChangesDataObject>> GetBcf21TopicsObjectsChanges(
        string projectId,
        string type,
        string changeToken,
        int pageSize = 100,
        string? skipToken = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/changes";
        var queryParams = new List<string>
        {
            $"type={type}",
            $"changeToken={changeToken}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrEmpty(skipToken))
        {
            queryParams.Add($"skiptoken={skipToken}");
        }

        url += "?" + string.Join("&", queryParams);

        var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.TopicsObjectsChanges.TopicsObjectsChangesDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = responseContent,
            Data = response.IsSuccessStatusCode ? 
                await JsonSerializer.DeserializeAsync<BCF21.v1.TopicsObjectsChanges.TopicsObjectsChangesDataObject>(
                    responseContent,
                    cancellationToken: cancellationToken) : null
        };
    }

    public async Task<ApiResponse<TopicsBatchDataObject>> CreateBcf21TopicsBatch(
        string projectId,
        CreateTopicsBatchActionInput input,
        string? validation = "strict",
        CancellationToken cancellationToken = default)
    {
        var url = $"bcf/2.1/projects/{projectId}/topics/batch";
        if (!string.IsNullOrEmpty(validation) && validation != "strict")
        {
            url += $"?validation={validation}";
        }

        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            url,
            input,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<TopicsBatchDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<TopicsBatchDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<TopicsBatchDataObject>> UpdateBcf21TopicsBatch(
        string projectId,
        UpdateTopicsBatchActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PatchAsync(
                $"bcf/2.1/projects/{projectId}/topics/batch",
                JsonContent.Create(input),
                cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<TopicsBatchDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<TopicsBatchDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<CommentsBatchDataObject>> CreateBcf21CommentsBatch(
        string projectId,
        CreateCommentsBatchActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/comments/batch",
            input,
            cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<CommentsBatchDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<CommentsBatchDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<CommentsBatchDataObject>> UpdateBcf21CommentsBatch(
        string projectId,
        UpdateCommentsBatchActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PatchAsync(
                $"bcf/2.1/projects/{projectId}/comments/batch",
                JsonContent.Create(input),
                cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<CommentsBatchDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<CommentsBatchDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<CurrentUserDataObject>> GetBcf21CurrentUser(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("bcf/2.1/current-user", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<CurrentUserDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<CurrentUserDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<UserClaimsDataObject>> GetBcf21UserClaims(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("api/claims", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<UserClaimsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<UserClaimsDataObject>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<IEnumerable<BCF21.v1.Viewpoints.ViewpointsDataObject>>> GetBcf21Viewpoints(
        string projectId,
        string topicId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints",
            cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<IEnumerable<BCF21.v1.Viewpoints.ViewpointsDataObject>>(content);
            
            return new ApiResponse<IEnumerable<BCF21.v1.Viewpoints.ViewpointsDataObject>>
            {
                IsSuccessful = true,
                StatusCode = (int)response.StatusCode,
                Data = data
            };
        }

        var rawResult = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return new ApiResponse<IEnumerable<BCF21.v1.Viewpoints.ViewpointsDataObject>>
        {
            IsSuccessful = false,
            StatusCode = (int)response.StatusCode,
            RawResult = rawResult
        };
    }

    public async Task<ApiResponse<BCF21.v1.Viewpoint.ViewpointDataObject>> GetBcf21Viewpoint(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}",
            cancellationToken).ConfigureAwait(false);

        var content = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var data = response.IsSuccessStatusCode ? 
            await JsonSerializer.DeserializeAsync<BCF21.v1.Viewpoint.ViewpointDataObject>(content, _jsonOptions, cancellationToken).ConfigureAwait(false) : 
            null;

        return new ApiResponse<BCF21.v1.Viewpoint.ViewpointDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = data,
            RawResult = response.IsSuccessStatusCode ? null : content
        };
    }

    public async Task<ApiResponse<BCF21.v1.Viewpoint.Create.CreateViewpointActionOutput>> CreateBcf21Viewpoint(
        string projectId,
        string topicId,
        CreateViewpointActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await System.Net.Http.Json.HttpClientJsonExtensions.PostAsJsonAsync(
            _httpClient,
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints",
            input,
            cancellationToken).ConfigureAwait(false);

        var content = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var data = response.IsSuccessStatusCode ? 
            await JsonSerializer.DeserializeAsync<BCF21.v1.Viewpoint.Create.CreateViewpointActionOutput>(content, _jsonOptions, cancellationToken).ConfigureAwait(false) : 
            null;

        return new ApiResponse<BCF21.v1.Viewpoint.Create.CreateViewpointActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = data,
            RawResult = response.IsSuccessStatusCode ? null : content
        };
    }

    public async Task<ApiResponse> DeleteBcf21Viewpoint(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false)
        };
    }

    public async Task<ApiResponse<byte[]>> GetBcf21ViewpointSnapshot(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/snapshot",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<byte[]>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? await response.Content.ReadAsByteArrayAsync(cancellationToken) : null,
            RawResult = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.ViewpointSelection.ViewpointSelectionDataObject>> GetBcf21ViewpointSelection(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/selection",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.ViewpointSelection.ViewpointSelectionDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.ViewpointSelection.ViewpointSelectionDataObject>(_jsonOptions, cancellationToken) : 
                default,
            RawResult = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.ViewpointColoring.ViewpointColoringDataObject>> GetBcf21ViewpointColoring(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/coloring",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.ViewpointColoring.ViewpointColoringDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.ViewpointColoring.ViewpointColoringDataObject>(_jsonOptions, cancellationToken) : 
                default,
            RawResult = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<BCF21.v1.ViewpointVisibility.ViewpointVisibilityDataObject>> GetBcf21ViewpointVisibility(
        string projectId,
        string topicId,
        string viewpointId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"bcf/2.1/projects/{projectId}/topics/{topicId}/viewpoints/{viewpointId}/visibility",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<BCF21.v1.ViewpointVisibility.ViewpointVisibilityDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<BCF21.v1.ViewpointVisibility.ViewpointVisibilityDataObject>(_jsonOptions, cancellationToken) : 
                default,
            RawResult = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    #endregion
}
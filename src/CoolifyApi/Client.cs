using Refit;

namespace CoolifyApi;

public interface ICoolifyApi
{
    [Get("/health")]
    Task<string> HealthcheckAsync();

    [Get("/servers")]
    Task<IReadOnlyList<Server>> ListServersAsync();

    [Get("/applications")]
    Task<IReadOnlyList<Application>> ListApplicationsAsync([AliasAs("tag")] string? tag = null);

    [Get("/projects")]
    Task<IReadOnlyList<Project>> ListProjectsAsync();

    [Get("/deployments")]
    Task<IReadOnlyList<Deployment>> ListDeploymentsAsync();

    [Get("/databases")]
    Task<string> ListDatabasesAsync();

    [Get("/resources")]
    Task<string> ListResourcesAsync();

    [Get("/services")]
    Task<IReadOnlyList<Service>> ListServicesAsync();
}

public sealed class Client
{
    private readonly ICoolifyApi _api;

    public Client(string apiKey, string baseUrl = "https://app.coolify.io/api/v1", HttpMessageHandler? messageHandler = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
        }

        var httpClient = messageHandler is null
            ? new HttpClient()
            : new HttpClient(messageHandler);

        httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        _api = RestService.For<ICoolifyApi>(httpClient);
    }

    public Client(HttpClient httpClient)
    {
        if (httpClient.BaseAddress is null)
        {
            throw new ArgumentException("HttpClient BaseAddress must be configured.", nameof(httpClient));
        }

        _api = RestService.For<ICoolifyApi>(httpClient);
    }

    public Task<string> HealthcheckAsync() => _api.HealthcheckAsync();

    public Task<IReadOnlyList<Server>> ListServersAsync() => _api.ListServersAsync();

    public Task<IReadOnlyList<Application>> ListApplicationsAsync(string? tag = null) => _api.ListApplicationsAsync(tag);

    public Task<IReadOnlyList<Project>> ListProjectsAsync() => _api.ListProjectsAsync();

    public Task<IReadOnlyList<Deployment>> ListDeploymentsAsync() => _api.ListDeploymentsAsync();

    public Task<string> ListDatabasesAsync() => _api.ListDatabasesAsync();

    public Task<string> ListResourcesAsync() => _api.ListResourcesAsync();

    public Task<IReadOnlyList<Service>> ListServicesAsync() => _api.ListServicesAsync();
}

public sealed record Server(string Uuid, string Name, string Ip, int Port, string User);

public sealed record Application(string Uuid, string Name, string BuildPack, string GitRepository, string GitBranch);

public sealed record Project(string Uuid, string Name, string? Description);

public sealed record Deployment(string DeploymentUuid, string Status, string ApplicationName, string ServerName);

public sealed record Service(string Uuid, string Name, string ServiceType);

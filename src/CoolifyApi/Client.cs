using Refit;

namespace CoolifyApi;

public sealed class CoolifyApiClient
{
    private const string DefaultBaseUrl = "https://app.coolify.io/api/v1";

    public ICoolifyApi Service { get; }

    public CoolifyApiClient(string apiKey, string baseUrl = DefaultBaseUrl, HttpMessageHandler? messageHandler = null)
        : this(CreateHttpClient(apiKey, baseUrl, messageHandler))
    {
    }

    public CoolifyApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        if (httpClient.BaseAddress is null)
        {
            throw new ArgumentException("HttpClient BaseAddress must be configured.", nameof(httpClient));
        }

        Service = RestService.For<ICoolifyApi>(httpClient);
    }

    private static HttpClient CreateHttpClient(string apiKey, string baseUrl, HttpMessageHandler? messageHandler)
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

        return httpClient;
    }

    public Task<string> HealthcheckAsync() => Service.Healthcheck();

    public async Task<IReadOnlyList<ApplicationSummary>> ListApplicationsAsync(string? tag = null)
    {
        var applications = await Service.ListApplications(tag ?? string.Empty);
        return applications
            .Select(a => new ApplicationSummary(a.Uuid, a.Name, a.BuildPack, a.Status, a.Fqdn))
            .ToArray();
    }

    public async Task<IReadOnlyList<ServerSummary>> ListServersAsync()
    {
        var servers = await Service.ListServers();
        return servers
            .Select(s => new ServerSummary(s.Uuid, s.Name, s.Ip, s.Port, s.User))
            .ToArray();
    }

    public async Task<IReadOnlyList<ProjectSummary>> ListProjectsAsync()
    {
        var projects = await Service.ListProjects();
        return projects
            .Select(p => new ProjectSummary(p.Uuid, p.Name, p.Description))
            .ToArray();
    }

    public async Task<IReadOnlyList<DeploymentSummary>> ListDeploymentsAsync()
    {
        var deployments = await Service.ListDeployments();
        return deployments
            .Select(d => new DeploymentSummary(d.DeploymentUuid, d.Status, d.ApplicationName, d.ServerName))
            .ToArray();
    }

    public Task<string> ListDatabasesAsync() => Service.ListDatabases();

    public Task<string> ListResourcesAsync() => Service.ListResources();

    public async Task<IReadOnlyList<ServiceSummary>> ListServicesAsync()
    {
        var services = await Service.ListServices();
        return services
            .Select(s => new ServiceSummary(s.Uuid, s.Name, s.ServiceType))
            .ToArray();
    }
}

public sealed record ApplicationSummary(string Uuid, string Name, string BuildPack, string Status, string Fqdn);

public sealed record ServerSummary(string Uuid, string Name, string Ip, int Port, string User);

public sealed record ProjectSummary(string Uuid, string Name, string Description);

public sealed record DeploymentSummary(string DeploymentUuid, string Status, string ApplicationName, string ServerName);

public sealed record ServiceSummary(string Uuid, string Name, string ServiceType);

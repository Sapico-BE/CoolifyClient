using CoolifyApi;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CoolifyApi.IntegrationTests;

public class CoolifyClientTests
{
    private readonly Client _client;

    public CoolifyClientTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<CoolifyClientTests>()
            .AddEnvironmentVariables()
            .Build();

        var apiKey = configuration["Coolify:ApiKey"]
            ?? throw new InvalidOperationException(
                "Coolify:ApiKey is not configured. Set it via user secrets: " +
                "dotnet user-secrets set \"Coolify:ApiKey\" \"<your-token>\"");

        var baseUrl = configuration["Coolify:BaseUrl"] ?? "https://app.coolify.io/api/v1";

        _client = new Client(apiKey, baseUrl);
    }

    [Fact]
    public async Task Healthcheck_ReturnsOk()
    {
        var result = await _client.HealthcheckAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task ListServers_ReturnsResult()
    {
        var servers = await _client.ListServersAsync();

        Assert.NotNull(servers);
    }

    [Fact]
    public async Task ListApplications_ReturnsResult()
    {
        var applications = await _client.ListApplicationsAsync(tag: null);

        Assert.NotNull(applications);
    }

    [Fact]
    public async Task ListProjects_ReturnsResult()
    {
        var projects = await _client.ListProjectsAsync();

        Assert.NotNull(projects);
    }

    [Fact]
    public async Task ListDeployments_ReturnsResult()
    {
        var deployments = await _client.ListDeploymentsAsync();

        Assert.NotNull(deployments);
    }

    [Fact]
    public async Task ListDatabases_ReturnsResult()
    {
        var databases = await _client.ListDatabasesAsync();

        Assert.NotNull(databases);
    }

    [Fact]
    public async Task ListResources_ReturnsResult()
    {
        var resources = await _client.ListResourcesAsync();

        Assert.NotNull(resources);
    }

    [Fact]
    public async Task ListServices_ReturnsResult()
    {
        var services = await _client.ListServicesAsync();

        Assert.NotNull(services);
    }
}

# CoolifyClient

A .NET client for the [Coolify API](https://coolify.io/docs/api-reference/authorization).

## Installation

```bash
dotnet add package CoolifyApi
```

## Usage

```csharp
using System.Net.Http.Headers;
using CoolifyApi;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", "your-api-token");

var client = new Client(httpClient)
{
    BaseUrl = "https://your-coolify-instance.com/api/v1"
};

var servers = await client.ListServersAsync();
var applications = await client.ListApplicationsAsync(tag: null);
var projects = await client.ListProjectsAsync();
```

## API Reference

For all available endpoints, see the [Coolify API docs](https://coolify.io/docs/api-reference/authorization).

## Integration Tests

Configure your API key via user secrets:

```bash
cd tests/CoolifyApi.IntegrationTests
dotnet user-secrets set "Coolify:ApiKey" "<your-token>"
dotnet user-secrets set "Coolify:BaseUrl" "https://your-coolify-instance.com/api/v1"
```

Then run:

```bash
dotnet test
```

## License

See [LICENSE](LICENSE).

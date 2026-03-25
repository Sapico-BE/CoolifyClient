# CoolifyClient

A .NET client for the [Coolify API](https://coolify.io/docs/api-reference/authorization), generated from `openapi.yaml` and then manually fine-tuned.

## Installation

```bash
dotnet add package CoolifyApi
```

## Usage

### Recommended (simplified facade)

```csharp
using CoolifyApi;

var client = new CoolifyApiClient(
    apiKey: "your-api-token",
    baseUrl: "https://your-coolify-instance.com/api/v1");

var health = await client.HealthcheckAsync();
var servers = await client.ListServersAsync();
var applications = await client.ListApplicationsAsync();
var projects = await client.ListProjectsAsync();
```

### Full generated surface (all endpoints)

```csharp
using CoolifyApi;

var client = new CoolifyApiClient("your-api-token", "https://your-coolify-instance.com/api/v1");

// Access the generated Refit interface directly:
var allTeams = await client.Service.ListTeams();
var app = await client.Service.GetApplicationByUuid("app-uuid");
```

## How this client is generated

The API interface and models in `src/CoolifyApi/ICoolifyApi.cs` are generated with `Refitter` from `src/openapi.yaml`.

Typical command used:

```bash
dotnet refitter src/openapi.yaml --skip-validation --simple-output
```

Notes:
- `--skip-validation` is used because the source OpenAPI document is `3.1.0`, while the generator validation path does not fully support it.
- Generated code is then committed into `ICoolifyApi.cs`.

## Manual fine-tuning applied

The exposed API behavior on Coolify `v4.0.0-beta.463` is not fully aligned with the shipped OpenAPI schema, so manual fixes were required:

1. **`Application.build_pack` compatibility fix**
   - The schema enum is too strict (`nixpacks`, `static`, `dockerfile`, `dockercompose`).
   - Real API responses include additional values (for example `dockerimage`).
   - To avoid deserialization failures, `Application.BuildPack` is treated as `string`.

2. **Simplified public API layer**
   - `CoolifyApiClient` exposes small summary models (`ApplicationSummary`, `ServerSummary`, etc.) for common operations.
   - The full generated interface is still available through `client.Service`.

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

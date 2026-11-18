# eBay Other APIs SDK for .NET

Identity (the user's account), eBay International Shipping, machine translation and VeRO reporting. This SDK wraps the eBay Identity API, eBay International Shipping (EDIS) API, Translation API and VeRO API v2 behind one client, `OtherApisClient`.

- **Target frameworks:** .NET Framework 4.7.2, .NET 8, .NET 9 and .NET 10
- **C#:** usable from C# 7.3 and later

## Installation

The package `EBay.OtherApis` is not on nuget.org yet. Reference the project, or build the package and add it to a local feed:

```bash
dotnet pack EBay.OtherApis -c Release -o artifacts/packages
```

## Quick start

```csharp
using EBay.OtherApis;
using EBay.OtherApis.Configuration;

var config = new OtherApisConfiguration
{
    // A user or application access token from your own OAuth flow (see "Authentication").
    AccessToken = Environment.GetEnvironmentVariable("EBAY_ACCESS_TOKEN")
};

using var client = new OtherApisClient(config);

var user = await client.Identity.GetUserAsync();
Console.WriteLine($"Signed in as {user?.Username}");
```

## APIs

| `OtherApisClient` property | API | Default base URL | Access token |
|---|---|---|---|
| `Identity` | Identity API | `https://apiz.ebay.com/commerce/identity/v1` | User |
| `InternationalShipping` | eBay International Shipping (EDIS) API | `https://api.ebay.com/sell/edelivery_international_shipping/v1` | User |
| `Translation` | Translation API | `https://api.ebay.com/commerce/translation/v1_beta` | Application or user |
| `VeroApiV2` | VeRO API v2 | `https://api.ebay.com/commerce/vero/v2` | Application or user |

"Access token" is what eBay's OpenAPI spec declares: an application token (client credentials) or a user token (authorization code). Scopes are listed per API below.

<details>
<summary><b>Identity</b>: Identity API, 1 operation</summary>

Scopes: `https://api.ebay.com/oauth/api_scope/commerce.identity.address.readonly`, `https://api.ebay.com/oauth/api_scope/commerce.identity.email.readonly`, `https://api.ebay.com/oauth/api_scope/commerce.identity.name.readonly`, `https://api.ebay.com/oauth/api_scope/commerce.identity.phone.readonly`, `https://api.ebay.com/oauth/api_scope/commerce.identity.readonly`, `https://api.ebay.com/oauth/api_scope/commerce.identity.status.readonly`

| Method | Description |
|---|---|
| `GetUserAsync` | This method retrieves the account profile information for an authenticated user, which requires a User access token. |

</details>

<details>
<summary><b>InternationalShipping</b>: eBay International Shipping (EDIS) API, 28 operations</summary>

Scopes: `https://api.ebay.com/oauth/scope/sell.edelivery`

> **Note:** This method is only available for Greater-China based sellers with an active eDIS account.

| Method | Description |
|---|---|
| `GetActualCostsAsync` | This method is used to retrieve the actual weight and cost for one or more packages based on the provided input criteria. |
| `GetAddressPreferencesAsync` | This method can be used to retrieve information about all the ship-from and/or return addresses associated with a seller's eDIS account. |
| `CreateAddressPreferenceAsync` | This method can be used to create an address on your eDIS account. |
| `GetAgentsAsync` | This method can be used to retrieve information about the EU Authorized Representative (AR) agent(s) associated with a seller's eDIS account. |
| `GetBatteryQualificationsAsync` | This method is used to retrieve all battery qualifications associated with your eDIS account. |
| `CancelBundleAsync` | This method is used to cancel a bundle. |
| `CreateBundleAsync` | This method is used to create a bundle for multiple packages in an order. |
| `GetBundleAsync` | This method is used to retrieved information about a specific bundle based on the provided bundle ID value. |
| `GetBundleLabelAsync` | This method can be used to retrieve the bundle label for a bundle of packages. |
| `CreateComplaintAsync` | This method can be used to create a complaint about a shipment order. |
| `GetConsignPreferencesAsync` | This method can be used to retrieve information about all the pickup, drop-off, and/or forward deployment consign addresses associated with a seller's eDIS account. |
| `CreateConsignPreferenceAsync` | This method can be used to create a consign address. |
| `GetCpscProfilesAsync` | This method can be used to retrieve all Consumer Product Safety Committee (CPSC) profiles associated with a user's account. |
| `GetDropoffSitesAsync` | This method can be used to retrieve metadata for one or more drop-off sites associated with a seller's eDIS account. |
| `GetHandoverSheetAsync` | This method can be used to retrieve the handover sheet for one or more packages in a pickup request. |
| `GetLabelsAsync` | This method can be used to retrieve shipping labels for the one or more specified packages. |
| `BulkCancelPackagesAsync` | This method can be used to cancel multiple packages and their associated tracking numbers. |
| `BulkConfirmPackagesAsync` | This method can be used to confirm the shipping order for multiple packages. |
| `BulkDeletePackagesAsync` | This method can be used to delete multiple packages. |
| `CancelPackageAsync` | This method can be used to cancel a package and its associated tracking number. |
| `ClonePackageAsync` | This method can be used to clone a package for redelivery, resending the package with the same information input during the createPackage call. |
| `ConfirmPackageAsync` | This method can be used to confirm the shipping order for a package. |
| `CreatePackageAsync` | This method can be used to create a package for an order. |
| `GetPackageAsync` | This method can be used to retrieve a package based on the provided package ID value. |
| `DeletePackageAsync` | This method can be used to delete a package. |
| `GetPackagesByLineItemIDAsync` | This method can be used to retrieve one or more packages based on the provided order line item ID. |
| `GetServicesAsync` | This method can be used to retrieve metadata on available shipping services, such as the name, description, and directions. |
| `GetTrackingAsync` | This method can be used to retrieve tracking event details for a package associated with the provided tracking number. |

</details>

<details>
<summary><b>Translation</b>: Translation API, 1 operation</summary>

Scopes: `https://api.ebay.com/oauth/api_scope`

| Method | Description |
|---|---|
| `TranslateAsync` | Translate text from one language to another |

</details>

<details>
<summary><b>VeroApiV2</b>: VeRO API v2, 5 operations</summary>

Scopes: `https://api.ebay.com/oauth/api_scope/commerce.vero`

> **Note:** You must be a member of the Verified Rights Owner (VeRO) Program to use this call.

| Method | Description |
|---|---|
| `GetVeroReasonCodeAsync` | This method is used to retrieve the details of a specific VeRO reason code, including a description of the code and the marketplace associated with the code. |
| `GetVeroReasonCodesAsync` | This method retrieves the details of an array of VeRO reason codes, including descriptions of each code and the marketplace associated with each code. |
| `ReportVeroViolationsAsync` | This method can be used to report a violation for an item that allegedly infringes on your copyright, trademark, or other intellectual property rights. |
| `GetVeroReportAsync` | This method retrieves status information about a VeRO report you have submitted to eBay for copyright, trademark, or other intellectual property rights infringement. |
| `GetVeroReportItemsAsync` | Retrieves status for VeRO infringement reports by item and date range. |

</details>

## Authentication

The SDK does not obtain or refresh tokens: get them with your own OAuth flow ([eBay OAuth guide](https://developer.ebay.com/develop/guides/sell/authorization)) and give them to the SDK in one of two ways:

- `OtherApisConfiguration.AccessToken`: used for every call that doesn't pass its own token.
- The `accessToken` parameter on every method: used for that call only, for example a user token where the configuration holds an application token.

```csharp
string userToken = Environment.GetEnvironmentVariable("EBAY_USER_TOKEN")!;
var user = await client.Identity.GetUserAsync(accessToken: userToken);
```

A token is only ever sent to trusted hosts, see below.

## Configuration

| Property | Type | Default | Description |
|---|---|---|---|
| `IdentityBaseUrl` | `string` | `https://apiz.ebay.com/commerce/identity/v1` | Base URL of the Identity API. |
| `InternationalShippingBaseUrl` | `string` | `https://api.ebay.com/sell/edelivery_international_shipping/v1` | Base URL of the eBay International Shipping (EDIS) API. |
| `TranslationBaseUrl` | `string` | `https://api.ebay.com/commerce/translation/v1_beta` | Base URL of the Translation API. |
| `VeroApiV2BaseUrl` | `string` | `https://api.ebay.com/commerce/vero/v2` | Base URL of the VeRO API v2. |
| `AccessToken` | `string?` |  | Fallback bearer token used when no per-call accessToken is supplied. |
| `DiagnosticLogging` | `bool` | `false` | When true, logs the HTTP method, URL, status code, and response reason phrase for all non-success responses to stderr. |
| `HttpTimeout` | `TimeSpan` | 30 seconds | Timeout applied to all HTTP requests. |
| `DefaultRequestOptions` | `RequestOptions` | no headers | Default headers applied to every REST request. |
| `AllowedHosts` | `IList<string>` | empty | Additional hosts trusted with the access token, for example a proxy (see below). |

`OtherApisConfiguration.DefaultAllowedHosts` lists the hosts trusted by default.

## Sandbox and trusted hosts

The access token is only sent to trusted hosts: the hosts in `OtherApisConfiguration.DefaultAllowedHosts`, which are the eBay production hosts and their `*.sandbox.ebay.com` counterparts, plus any in `AllowedHosts`. Switching to the sandbox needs only the base URLs:

```csharp
var sandboxConfig = new OtherApisConfiguration
{
    AccessToken = Environment.GetEnvironmentVariable("EBAY_SANDBOX_ACCESS_TOKEN"),
    IdentityBaseUrl = "https://apiz.sandbox.ebay.com/commerce/identity/v1",
    InternationalShippingBaseUrl = "https://api.sandbox.ebay.com/sell/edelivery_international_shipping/v1",
    TranslationBaseUrl = "https://api.sandbox.ebay.com/commerce/translation/v1_beta",
    VeroApiV2BaseUrl = "https://api.sandbox.ebay.com/commerce/vero/v2",
};
```

Any other host, for example a proxy that forwards to eBay, has to be added to `AllowedHosts`; the client constructor throws `InvalidOperationException` otherwise:

```csharp
var proxied = new OtherApisConfiguration
{
    IdentityBaseUrl = "https://ebay-proxy.example.com/commerce/identity/v1",
    AllowedHosts = { "ebay-proxy.example.com" },
};
```

## Dependency injection

`AddOtherApisSdk` registers the configuration and one shared `OtherApisClient` as a singleton. It owns its HTTP connections and is disposed with the container. Inject the client and use its properties for the individual APIs:

```csharp
using EBay.OtherApis;
using EBay.OtherApis.Configuration;
using EBay.OtherApis.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOtherApisSdk(new OtherApisConfiguration
{
    AccessToken = builder.Configuration["EBay:AccessToken"]
});

var app = builder.Build();
app.MapGet("/example", async (OtherApisClient client) =>
{
    var user = await client.Identity.GetUserAsync();
    return Results.Ok();
});
app.Run();
```

## Custom HTTP handler

To send requests through your own handler, for example for a proxy, client certificates or a test double, pass it to the constructor. The handler is shared by all APIs and is not disposed by the client:

```csharp
var handler = new HttpClientHandler { Proxy = new WebProxy("http://corporate-proxy:8080") };
using var proxiedClient = new OtherApisClient(config, handler);
```

## Error handling

```csharp
try
{
    var user = await client.Identity.GetUserAsync();
}
catch (UnauthorizedException)
{
    // HTTP 401: the token is missing, expired, for the other environment, or lacks a scope.
}
catch (ApiException ex)
{
    // Any other REST error.
    Console.WriteLine($"HTTP {ex.ResponseStatusCode}");
}
```

- `UnauthorizedException` (HTTP 401) derives from `SdkException`.
- Other REST errors throw Kiota's `ApiException`, or a generated subclass named in eBay's spec for that operation, with `ResponseStatusCode`. eBay's error text is only available where the spec describes the error body correctly; set `DiagnosticLogging = true` to log failed requests to stderr.

## .NET Framework

The `net472` build is a classic package that `packages.config` projects can use. Such projects must list every dependency themselves. At the minimum versions the SDK supports, these are:

| Package | Version |
|---|---|
| Microsoft.Kiota.Abstractions, .Bundle, .Http.HttpClientLibrary, .Serialization.Form, .Serialization.Json, .Serialization.Multipart, .Serialization.Text | 1.22.0 |
| Std.UriTemplate | 2.0.8 |
| Microsoft.IO.RecyclableMemoryStream | 3.0.1 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0 |
| System.Diagnostics.DiagnosticSource, System.Net.Http.WinHttpHandler | 6.0.0 |
| System.Text.Json, System.Text.Encodings.Web, System.IO.Pipelines, Microsoft.Bcl.AsyncInterfaces | 10.0.0 |
| System.Runtime.CompilerServices.Unsafe | 6.1.2 |
| System.Memory, System.Threading.Tasks.Extensions | 4.6.3 |
| System.Buffers, System.Numerics.Vectors, System.ValueTuple | 4.6.1 |

With these versions the app needs binding redirects for Microsoft.Bcl.AsyncInterfaces (10.0.0.0), System.Buffers (4.0.5.0), System.Memory (4.0.5.0), System.Runtime.CompilerServices.Unsafe (6.0.3.0), System.Text.Encodings.Web (10.0.0.0), System.Text.Json (10.0.0.0) and System.Threading.Tasks.Extensions (4.2.4.0). There must be no redirect for System.ValueTuple: .NET Framework 4.7.1+ provides it. Newer package versions work too and change the redirect versions.

## Development

- .NET 10 SDK, plus the .NET 8 and .NET 9 SDKs so every target framework builds. `net472` builds on any OS; its tests run on Windows only.
- Build: `dotnet build EBay.OtherApis`
- Test: `dotnet test tests/EBay.OtherApis.Tests`
- Most of this repository is generated from eBay's API specs by the SDK generator (`api-sdk-definitions`): the API clients, `Configuration/`, `Extensions/`, `OtherApisClient.cs`, `specs/` and `generate.sh`. Change the generator, not the generated files. `./generate.sh` reruns Kiota for the specs in `specs/openapi`.

See [CONTRIBUTING.adoc](CONTRIBUTING.adoc).

## License

Apache 2.0, see [LICENSE](LICENSE).

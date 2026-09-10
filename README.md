# EBay.OtherApis .NET SDK

A .NET SDK for eBay Other APIs. Wraps four REST APIs (Kiota-generated) behind a single unified client.

## Target framework

`net10.0`

## What's included

| Client | API | Transport |
|---|---|---|
| `Shipping` | EDIS Shipping API (sell_edelivery_international_shipping) | REST (Kiota) |
| `Identity` | Commerce Identity API | REST (Kiota) |
| `Translation` | Commerce Translation API (beta) | REST (Kiota) |
| `Vero` | Commerce Vero API | REST (Kiota) |

All four are exposed on `OtherApisClient` and can also be injected directly via DI.

## APIs Overview

### EDIS Shipping API (28 operations)
Package management, bundles, labels, tracking, and shipping preferences for international shipping:
- Package operations: create, get, update, delete, cancel, confirm, clone
- Bundle operations: create, get, cancel
- Labels and tracking
- Preferences: address and consignment
- Agents, dropoff sites, services

### Identity API (1 operation)
User identity management:
- Get user information

### Translation API (1 operation)
Text translation services:
- Translate text between languages

### Vero API (5 operations)
VERO (Verified Rights Owner) reporting:
- Create and manage VERO reports
- Get VERO reason codes
- Manage reported items

## Authentication

Tokens are passed per call. Set a global fallback on the config for convenience:

```csharp
var config = new OtherApisConfiguration
{
    AccessToken = "v^1.1-..."   // fallback when no per-call token is supplied
};
```

Per-call token takes precedence; `AccessToken` is the fallback. If neither is set, the SDK throws at call time.

## Usage

### Without DI (scripts, tests)

```csharp
using EBay.OtherApis;
using EBay.OtherApis.Configuration;

var config = new OtherApisConfiguration { AccessToken = "v^1.1-..." };

using var client = new OtherApisClient(config);

// EDIS Shipping
var package = await client.Shipping.CreatePackageAsync(...);
var tracking = await client.Shipping.GetTrackingAsync();

// Identity
var user = await client.Identity.GetUserAsync();

// Translation
var translated = await client.Translation.TranslateAsync(...);

// Vero
var report = await client.Vero.CreateVeroReportAsync(...);
```

### With DI (ASP.NET Core)

```csharp
// Program.cs
builder.Services.AddOtherApisSdk(new OtherApisConfiguration
{
    AccessToken = builder.Configuration["EBay:AccessToken"]
});
```

Then inject any client directly:

```csharp
app.MapGet("/packages", async (ShippingClient client) =>
    Results.Ok(await client.GetPackagesAsync()));

app.MapGet("/user", async (IdentityClient client) =>
    Results.Ok(await client.GetUserAsync()));

app.MapPost("/translate", async (TranslationClient client) =>
    Results.Ok(await client.TranslateAsync(...)));

app.MapGet("/vero-reason-codes", async (VeroClient client) =>
    Results.Ok(await client.GetVeroReasonCodesAsync()));
```

### Sandbox

Override base URLs on `OtherApisConfiguration`:

```csharp
var config = new OtherApisConfiguration
{
    AccessToken = "...",
    ShippingBaseUrl = "https://api.sandbox.ebay.com/sell/edelivery/international_shipping/v1",
    IdentityBaseUrl = "https://apim.sandbox.ebay.com/commerce/identity/v1",
    TranslationBaseUrl = "https://apim.sandbox.ebay.com/commerce/translation/v1_beta",
    VeroBaseUrl = "https://apim.sandbox.ebay.com/commerce/vero/v1",
};
```

## Code generation

Regenerate REST clients from updated OpenAPI specs:

```bash
./generate.sh
```

## License

[Apache 2.0](LICENSE) © 2026 eBay Inc.

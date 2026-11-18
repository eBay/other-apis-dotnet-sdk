# EBay.OtherApis .NET SDK

A .NET SDK for eBay Other APIs. Wraps four REST APIs (Kiota-generated) behind a single unified client.

## Target framework

`net472` (.NET Framework 4.7.2), `net8.0`, `net9.0` and `net10.0`

## What's included

| Client | API | Transport |
|---|---|---|
| `InternationalShipping` | EDIS Shipping API (sell_edelivery_international_shipping) | REST (Kiota) |
| `Identity` | Commerce Identity API | REST (Kiota) |
| `Translation` | Commerce Translation API (beta) | REST (Kiota) |
| `VeroApiV2` | Commerce VeRO API v2 | REST (Kiota) |

All four are exposed as properties of `OtherApisClient`. The VeRO API v1 (`Vero`) was removed in 0.4.0: eBay decommissioned it on September 30, 2026. Use `VeroApiV2`.

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

### VeRO API v2 (`client.VeroApiV2`)

eBay decommissioned the VeRO API v1 on September 30, 2026; it was removed from this SDK in 0.4.0.

## Trusted hosts and sandbox

The access token is only sent to trusted hosts: the eBay hosts in `OtherApisConfiguration.DefaultAllowedHosts` (production and their `*.sandbox.ebay.com` counterparts) plus any you add to `AllowedHosts`. The sandbox needs no extra configuration:

```csharp
var config = new OtherApisConfiguration
{
    AccessToken = "...",
    IdentityBaseUrl = "https://apiz.sandbox.ebay.com/commerce/identity/v1",
};
```

Any other host, for example a proxy, has to be added explicitly; the client constructor throws `InvalidOperationException` otherwise:

```csharp
config.IdentityBaseUrl = "https://ebay-proxy.example.com/commerce/identity/v1";
config.AllowedHosts.Add("ebay-proxy.example.com");
```

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
var package = await client.InternationalShipping.CreatePackageAsync(...);
var tracking = await client.InternationalShipping.GetTrackingAsync();

// Identity
var user = await client.Identity.GetUserAsync();

// Translation
var translated = await client.Translation.TranslateAsync(...);

```

### With DI (ASP.NET Core)

`AddOtherApisSdk` registers the configuration and one shared `OtherApisClient` (a singleton that owns its HTTP connections and is disposed with the container). Inject the client and use its properties for the individual APIs:

```csharp
using EBay.OtherApis;
using EBay.OtherApis.Configuration;
using EBay.OtherApis.Extensions;

builder.Services.AddOtherApisSdk(new OtherApisConfiguration
{
    AccessToken = builder.Configuration["EBay:AccessToken"]
});

app.MapGet("/example", async (OtherApisClient sdk) =>
    Results.Ok(await sdk.Identity.GetUserAsync()));
```

To route requests through your own handler (proxy, client certificates, a test double), pass it to the constructor: `new OtherApisClient(config, handler)`. The handler is shared by all APIs and is not disposed by the client.

### Sandbox

Override base URLs on `OtherApisConfiguration`; the sandbox hosts are trusted by default:

```csharp
var config = new OtherApisConfiguration
{
    AccessToken = "...",
    InternationalShippingBaseUrl = "https://api.sandbox.ebay.com/sell/edelivery_international_shipping/v1",
    IdentityBaseUrl = "https://apiz.sandbox.ebay.com/commerce/identity/v1",
    TranslationBaseUrl = "https://api.sandbox.ebay.com/commerce/translation/v1_beta",
    VeroApiV2BaseUrl = "https://api.sandbox.ebay.com/commerce/vero/v2",
};
```

## Code generation

Regenerate REST clients from updated OpenAPI specs:

```bash
./generate.sh
```

## Dependencies

- **Microsoft.Kiota.Bundle** (>= 1.22.0): REST runtime
- **Microsoft.Extensions.DependencyInjection.Abstractions** (>= 6.0.0): `AddOtherApisSdk` registration

## License

[Apache 2.0](LICENSE) © 2026 eBay Inc.

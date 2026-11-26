# ChangeLog

## 0.5.0 (11/26/2026)

### Added

- Local mock servers: base URL properties accept plain `http` when the host is a loopback host (`127.0.0.1`, `::1` or `localhost`) that you also add to `OtherApisConfiguration.AllowedHosts`. Before, every base URL had to use HTTPS. Every other host still requires HTTPS. `OtherApisConfiguration.LoopbackHosts` lists the loopback hosts.

### Changed

- Hosts are compared case-insensitively and without IPv6 brackets, so `[::1]` and `::1` match.

## 0.4.0 (11/17/2026)

### Breaking changes

#### VeRO API v1 removed

eBay decommissioned the VeRO API v1 on September 30, 2026. `VeroClient`, `OtherApisClient.Vero`, `OtherApisConfiguration.VeroBaseUrl` and the `EBay.OtherApis.Generated.Vero` models are removed. Use the VeRO API v2 (`OtherApisClient.VeroApiV2`).

#### International Shipping enum values renumbered

eBay added values to several International Shipping enums, which changes the numeric values of existing members. Code that stores or compares these enums as numbers must be updated; code that uses the member names is unaffected.

- `PackageStatusEnum`: new `PREDECLARATION_FAILED`, `INCOMPLETE_PACKAGE_INFO`, `NO_AVAILABLE_SHIPPING`, `PENDING_SHIPPING_SELECTION`, `PENDING_TRACKING_NUMBER`, `PENDING_CONFIRM`, `AWAITING_PICKUP`, `IN_TRANSIT`, `SHIPPING_EXCEPTION`, `DELIVERY_FAILED`, `FDC_VALIDATION_FAILED`, `FDC_VALIDATION_IN_PROGRESS`
- `ConsignTypeEnum`: new `RDC_DROP_OFF`
- `LiBatteryTypeEnum`: new `DRY_BATTERY_PACKED_WITH_EQUIPMENT`
- `ComplaintTypeEnum`: new `ABNORMAL_PICKUP_COMPLAINT`

#### Simpler dependency injection

- `AddOtherApisSdk` now registers only the configuration and a singleton `OtherApisClient`. `IdentityClient`, `InternationalShippingClient`, `TranslationClient` and `VeroApiV2Client` are no longer registered individually: inject `OtherApisClient` and use its `Identity`, `InternationalShipping`, `Translation` and `VeroApiV2` properties.
- The SDK no longer uses `IHttpClientFactory` or registers named `HttpClient`s, and no longer depends on `Microsoft.Extensions.Http`. To customise HTTP (proxy, client certificates, test doubles), use the new `OtherApisClient(OtherApisConfiguration, HttpMessageHandler)` constructor.

#### Trusted hosts

- The client constructors throw `InvalidOperationException` when a base URL uses a host that is not trusted (see Security). Add such hosts, for example a proxy, to `OtherApisConfiguration.AllowedHosts`.

### Security

- The access token is sent only to trusted hosts. Before, it was attached to requests for any HTTPS host configured as a base URL.

### Added

- `OtherApisConfiguration.AllowedHosts` for additional hosts trusted with the access token, and `OtherApisConfiguration.DefaultAllowedHosts`. The eBay production hosts and their sandbox counterparts (for example `api.sandbox.ebay.com`) are trusted by default, so the sandbox needs no extra configuration.
- Target framework `net472` (.NET Framework 4.7.2).
- `SkuRequest.AdditionalSkuInfo` (`AdditionalSkuInfo` name/value pairs) in the International Shipping API.

### Changed

- Dependency minimums are lowered to the versions Kiota itself requires, so projects can keep older compatible versions: `Microsoft.Kiota.Bundle` >= 1.22.0 and `Microsoft.Extensions.DependencyInjection.Abstractions` >= 6.0.0. Previously the SDK required the newest 10.0.x packages. On .NET Framework the SDK requires `System.Text.Json` >= 10.0.0, matching the oldest supported .NET LTS (.NET 10). None of these minimums has a known vulnerability, and each release is tested at both the minimum and the latest versions.
- On .NET Framework the SDK's HttpClients allow 64 connections per host instead of the `ServicePointManager` default of 2.
- `OtherApisConfiguration.HttpTimeout` and `RequestOptions.Headers` have `set` accessors instead of `init`, so projects compiled with C# 7.3 can assign them. Existing source compiles unchanged; recompile code that was built against 0.3.0.

## 0.3.0 (09/25/2026)

### Added

- Target frameworks `net8.0` and `net9.0`, in addition to `net10.0`.

## 0.2.0 (09/14/2026)

### Breaking changes

#### Removed clients

- `ShippingClient`

#### Changed signatures

`VeroClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `GetVeroReasonCodesAsync` | `(string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? xEBAYCMARKETPLACEID, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetVeroReportAsync` | `(string veroReportId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string veroReportId, string? includeItemDetails, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetVeroReportItemsAsync` | `(string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? filter, string? itemId, string? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |

### Added

- Client `InternationalShippingClient`
- Client `VeroApiV2Client`
- `InternationalShippingClient.GetCpscProfilesAsync(string? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)`
- Member `OtherApisClient.VeroApiV2`
- Member `OtherApisConfiguration.VeroApiV2BaseUrl`

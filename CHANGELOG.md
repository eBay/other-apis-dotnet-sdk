# ChangeLog

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

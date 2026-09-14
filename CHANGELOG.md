# ChangeLog

## 0.2.0 (09/14/2026)

### Breaking changes

#### Removed clients

- `ShippingClient`

#### Changed signatures

`VeroClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `GetVeroReasonCodesAsync` | `(string?, RequestOptions?, CancellationToken)` | `(string?, string?, RequestOptions?, CancellationToken)` |
| `GetVeroReportAsync` | `(string, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, RequestOptions?, CancellationToken)` |
| `GetVeroReportItemsAsync` | `(string?, RequestOptions?, CancellationToken)` | `(string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |

### Added

- Client `InternationalShippingClient`
- Client `VeroApiV2Client`
- `InternationalShippingClient.GetCpscProfilesAsync(string?, string?, string?, RequestOptions?, CancellationToken)`
- Member `OtherApisClient.VeroApiV2`
- Member `OtherApisConfiguration.VeroApiV2BaseUrl`

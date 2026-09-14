# ChangeLog

## 1.1.0 (09/14/2026)

- The following clients were added:
  - `VeroApiV2Client`
- The following operations were added to `InternationalShippingClient`:
  - `GetCpscProfilesAsync(string?, string?, string?, RequestOptions?, CancellationToken)`
- The following members were added to `OtherApisClient`:
  - `VeroApiV2`
- The following members were added to `OtherApisConfiguration`:
  - `VeroApiV2BaseUrl`

## 1.0.0 (09/14/2026)

**Breaking changes**
- The following clients were removed:
  - `ShippingClient`
- The following operations changed signature on `VeroClient`:
  - `GetVeroReasonCodesAsync`: `(string?, RequestOptions?, CancellationToken)` → `(string?, string?, RequestOptions?, CancellationToken)`
  - `GetVeroReportAsync`: `(string, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `GetVeroReportItemsAsync`: `(string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`

- The following clients were added:
  - `InternationalShippingClient`

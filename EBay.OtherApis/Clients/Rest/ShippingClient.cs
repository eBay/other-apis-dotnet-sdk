// Copyright 2026 eBay Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using EBay.OtherApis.Configuration;
using EBay.OtherApis.Generated.Shipping;
using EBay.OtherApis.Generated.Shipping.Models;
using EBay.OtherApis.Generated.Shipping.Actual_costs;
using EBay.OtherApis.Generated.Shipping.Dropoff_sites;
using EBay.OtherApis.Generated.Shipping.Handover_sheet;
using EBay.OtherApis.Generated.Shipping.Labels;
using EBay.OtherApis.Generated.Shipping.Tracking;
using EBay.OtherApis.Generated.Shipping.Complaint;
using EBay.OtherApis.Exceptions;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

/// <summary>
/// Wrapper for EDIS Shipping API (sell_edelivery_international_shipping).
/// Provides methods for package management, bundles, labels, and tracking.
/// </summary>
public sealed class ShippingClient
{
    private readonly ShippingApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal ShippingClient(ShippingApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    // ── Package Operations ────────────────────────────────────────────────────

    public async Task<AddPackageResponses?> CreatePackageAsync(AddPackageRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetPackageDetailResponses?> GetPackageAsync(string packageId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package[packageId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task DeletePackageAsync(string packageId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package[packageId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task CancelPackageAsync(string packageId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package[packageId].Cancel.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task ConfirmPackageAsync(string packageId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package[packageId].Confirm.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<ClonePackageResponses?> ClonePackageAsync(string packageId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package[packageId].Clone.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetItemPackageIdResponses?> GetPackagesByLineItemIDAsync(string orderLineItemId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package[orderLineItemId].Item.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Bulk Package Operations ───────────────────────────────────────────────

    public async Task<CancelPackagesResponses?> BulkCancelPackagesAsync(CancelPackagesRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package.Bulk_cancel_packages.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<ConfirmPackagesResponses?> BulkConfirmPackagesAsync(ConfirmPackagesRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package.Bulk_confirm_packages.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<DeletePackagesResponses?> BulkDeletePackagesAsync(DeletePackagesRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Package.Bulk_delete_packages.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Bundle Operations ─────────────────────────────────────────────────────

    public async Task<CreateBundleResponse?> CreateBundleAsync(CreateBundleRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Bundle.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<BundleDetailResponse?> GetBundleAsync(string bundleId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Bundle[bundleId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task CancelBundleAsync(string bundleId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Bundle[bundleId].Cancel.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<BundleLabelResponse?> GetBundleLabelAsync(string bundleId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Bundle[bundleId].Label.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Preferences ───────────────────────────────────────────────────────────

    public async Task<CreateAddressPreferenceResponses?> CreateAddressPreferenceAsync(CreateAddressPreferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Address_preference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetAddressPreferenceListResponses?> GetAddressPreferencesAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Address_preference.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<CreateConsignPreferenceResponses?> CreateConsignPreferenceAsync(CreateConsignPreferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Consign_preference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetConsignPreferenceListResponses?> GetConsignPreferencesAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Consign_preference.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Other Operations ──────────────────────────────────────────────────────

    public async Task<GetActualCostResponses?> GetActualCostsAsync(
        Action<RequestConfiguration<EBay.OtherApis.Generated.Shipping.Actual_costs.Actual_costsRequestBuilder.Actual_costsRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Actual_costs.GetAsync(cfg =>
                {
                    configure?.Invoke(cfg);
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetAgentListResponses?> GetAgentsAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Agents.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetBatteryQualListResponses?> GetBatteryQualificationsAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Battery_qualifications.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetDropoffSiteListResponses?> GetDropoffSitesAsync(
        Action<RequestConfiguration<EBay.OtherApis.Generated.Shipping.Dropoff_sites.Dropoff_sitesRequestBuilder.Dropoff_sitesRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Dropoff_sites.GetAsync(cfg =>
                {
                    configure?.Invoke(cfg);
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetHandoverSheetResponses?> GetHandoverSheetAsync(
        Action<RequestConfiguration<EBay.OtherApis.Generated.Shipping.Handover_sheet.Handover_sheetRequestBuilder.Handover_sheetRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Handover_sheet.GetAsync(cfg =>
                {
                    configure?.Invoke(cfg);
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetLabelListResponses?> GetLabelsAsync(
        Action<RequestConfiguration<EBay.OtherApis.Generated.Shipping.Labels.LabelsRequestBuilder.LabelsRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Labels.GetAsync(cfg =>
                {
                    configure?.Invoke(cfg);
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetServiceListResponses?> GetServicesAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Services.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<GetTrackingDetailResponses?> GetTrackingAsync(
        Action<RequestConfiguration<EBay.OtherApis.Generated.Shipping.Tracking.TrackingRequestBuilder.TrackingRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Tracking.GetAsync(cfg =>
                {
                    configure?.Invoke(cfg);
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    public async Task<ComplaintPostResponse?> CreateComplaintAsync(AddComplaintRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Complaint.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }
}

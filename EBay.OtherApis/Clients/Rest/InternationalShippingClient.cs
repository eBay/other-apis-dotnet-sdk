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
using EBay.OtherApis.Generated.InternationalShipping.Models;
using EBay.OtherApis.Generated.InternationalShipping;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

public sealed class InternationalShippingClient
{
    private readonly InternationalShippingApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal InternationalShippingClient(InternationalShippingApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method is used to retrieve the actual weight and cost for one or more packages based on the provided input criteria.
    /// Responses can be filtered by the <b>tracking_numbers</b> query parameter. If this parameter is used, actual costs for each package associated with the provided tracking numbers are returned.
    /// Alternatively, responses can be filtered by the <b>trans_begin_time</b> and <b>trans_end_time</b> query parameters. If these parameters are used, the actual costs of all packages created within this date range are returned.
    /// Only one filtering method can be used per call.
    /// </summary>
    public Task<GetActualCostResponses?> GetActualCostsAsync(
        string? trackingNumbers = null,
        string? transBeginTime = null,
        string? transEndTime = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Actual_costs.GetAsync(cfg =>
        {
            if (trackingNumbers is not null)
            {
                cfg.QueryParameters.TrackingNumbers = trackingNumbers;
            }
            if (transBeginTime is not null)
            {
                cfg.QueryParameters.TransBeginTime = transBeginTime;
            }
            if (transEndTime is not null)
            {
                cfg.QueryParameters.TransEndTime = transEndTime;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve information about all the ship-from and/or return addresses associated with a seller's eDIS account.
    /// </summary>
    public Task<GetAddressPreferenceListResponses?> GetAddressPreferencesAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Address_preference.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to create an address on your eDIS account.
    /// The following types of addresses can be created through this method:
    /// - <c>SHIP_FROM_ADDRESS</c>
    /// - <c>RETURN_ADDRESS</c>
    /// The address information for the address, including location details, contact information, and address type, must be provided in the <b>shipFromAddress</b> container. If the call is successful, the <b>addressId</b> for the newly created address will be returned. This identifier can then be used when creating a package to specify the address to use for the shipment.
    /// </summary>
    public Task<CreateAddressPreferenceResponses?> CreateAddressPreferenceAsync(
        CreateAddressPreferenceRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Address_preference.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve information about the EU Authorized Representative (AR) agent(s) associated with a seller's eDIS account.
    /// An EU AR agent acts as a liaison between non-EU manufacturers and European regulatory authorities to ensure compliance with EU regulations. This method returns contact information about each agent, as well as their eBay ID and the countries that they support.
    /// </summary>
    public Task<GetAgentListResponses?> GetAgentsAsync(
        string? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Agents.GetAsync(cfg =>
        {
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method is used to retrieve all battery qualifications associated with your eDIS account. This includes the type, electronic qualification ID and name, and expiration date for each battery.
    /// </summary>
    public Task<GetBatteryQualListResponses?> GetBatteryQualificationsAsync(
        string? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Battery_qualifications.GetAsync(cfg =>
        {
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method is used to cancel a bundle.
    /// The <b>bundleId</b> value of the bundle to be canceled must be input as a path parameter in the request.
    /// <b>Note:</b> Bundles cannot be canceled if they are currently being processed.
    /// </summary>
    public Task CancelBundleAsync(
        string bundleId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Bundle[bundleId].Cancel.PostAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method is used to create a bundle for multiple packages in an order.
    /// To create a bundle, the consign address to be associated with the bundle and the tracking numbers of the packages to be used in the bundle must be specified in the request.
    /// If the call is successful, the <b>bundleId</b> for the newly created bundle will be returned.
    /// <b>Note:</b> Sellers should keep track of the returned <b>bundleId</b>, as there is no programmatic way to retrieve this value at this time.
    /// </summary>
    public Task<CreateBundleResponse?> CreateBundleAsync(
        CreateBundleRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Bundle.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method is used to retrieved information about a specific bundle based on the provided bundle ID value.
    /// The <b>bundle_id</b> value of the bundle to be retrieved must be input as a path parameter in the request. If the call is successful, information about the bundle is returned, such as its associated consign preference ID and the tracking numbers of each package in the bundle.
    /// </summary>
    public Task<BundleDetailResponse?> GetBundleAsync(
        string bundleId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Bundle[bundleId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve the bundle label for a bundle of packages.
    /// The bundle for which to retrieve a bundle label is specified through the <b>bundleId</b> path parameter.
    /// Bundle labels will be returned as base64 string values which can be converted to PDF files.
    /// </summary>
    public Task<BundleLabelResponse?> GetBundleLabelAsync(
        string bundleId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Bundle[bundleId].Label.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to create a complaint about a shipment order. Complaints can be filed if there is a pickup delay on an order or if an item has been lost in transit.
    /// The complaint reason, complaint type, and the date the issue occurred must be specified in the request. If the complaint is being filed for a missing package, the affected <b>packageId</b> values must also be specified in the <b>affectedPackages</b> array.
    /// If the call was successful, HTTP status code <b>201 Created</b> will be returned, and the complaint will be created.
    /// </summary>
    public Task CreateComplaintAsync(
        AddComplaintRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Complaint.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve information about all the pickup, drop-off, and/or forward deployment consign addresses associated with a seller's eDIS account.
    /// </summary>
    public Task<GetConsignPreferenceListResponses?> GetConsignPreferencesAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Consign_preference.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to create a consign address.
    /// The following types of address preferences can be created via this method:
    /// - <c>PICK_UP</c>
    /// - <c>DROP_OFF</c>
    /// - <c>FORWARD_DEPLOYMENT</c>
    /// - <c>RDC</c>
    /// - <c>CN_POST_DROP_OFF</c>
    /// - <c>CN_POST_PICK_UP</c>
    /// - <c>HK_POST_DROP_OFF</c>
    /// The address information for the consign address must be provided in the <b>consignAddress</b> container. If the call is successful, the <b>addressId</b> for the newly created address will be returned. This ID can then be used when creating a package to specify the consign address to use for the shipment.
    /// </summary>
    public Task<CreateConsignPreferenceResponses?> CreateConsignPreferenceAsync(
        CreateConsignPreferenceRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Consign_preference.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve metadata for one or more drop-off sites associated with a seller's eDIS account.
    /// Pagination query parameters are provided that allow users to control how many drop-off site locations are returned in the response.
    /// </summary>
    public Task<GetDropoffSiteListResponses?> GetDropoffSitesAsync(
        string? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Dropoff_sites.GetAsync(cfg =>
        {
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve the handover sheet for one or more packages in a pickup request.
    /// The tracking numbers for the packages for which to retrieve handover sheets are specified through the <b>tracking_numbers</b> query parameter.
    /// The retrieved handover sheet will be returned as a base64 string value, and can be used for confirmation with a pickup contact. Only one handover sheet will be returned for the specified packages.
    /// </summary>
    public Task<GetHandoverSheetResponses?> GetHandoverSheetAsync(
        string trackingNumbers,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Handover_sheet.GetAsync(cfg =>
        {
            cfg.QueryParameters.TrackingNumbers = trackingNumbers;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve shipping labels for the one or more specified packages.
    /// The packages for which to retrieve shipping labels can be specified through the <b>tracking_numbers</b> query parameter. The page size and print preference settings for the shipping label(s) can also be specified through the query parameters.
    /// Shipping labels are returned as base64 string values which can be converted to PDF files. A separate file will be returned for each tracking number specified in the request.
    /// </summary>
    public Task<GetLabelListResponses?> GetLabelsAsync(
        string trackingNumbers,
        string? pageSize = null,
        string? printPreference = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Labels.GetAsync(cfg =>
        {
            cfg.QueryParameters.TrackingNumbers = trackingNumbers;
            if (pageSize is not null)
            {
                cfg.QueryParameters.PageSize = pageSize;
            }
            if (printPreference is not null)
            {
                cfg.QueryParameters.PrintPreference = printPreference;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to cancel multiple packages and their associated tracking numbers.
    /// The package ID values for the packages to be canceled must be input in the <b>packageIds</b> array of the request.
    /// <b>Note:</b> Packages cannot be cancelled if they are currently being processed.
    /// </summary>
    public Task<CancelPackagesResponses?> BulkCancelPackagesAsync(
        CancelPackagesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package.Bulk_cancel_packages.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to confirm the shipping order for multiple packages.
    /// The package ID values for the packages to be confirmed must be input in the <b>packageIds</b> array of the request.
    /// After a package is confirmed, the estimated cost will be frozen in your wallet and the pickup request will be submitted.
    /// </summary>
    public Task<ConfirmPackagesResponses?> BulkConfirmPackagesAsync(
        ConfirmPackagesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package.Bulk_confirm_packages.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to delete multiple packages.
    /// The package ID values for the packages to be deleted must be input in the <b>packageIds</b> array of the request.
    /// <b>Note:</b> Packages can only be deleted once they have been cancelled through the <b>cancelPackage</b> or <b>bulkCancelPackages</b> methods and no longer have a tracking number associated with them.
    /// </summary>
    public Task<DeletePackagesResponses?> BulkDeletePackagesAsync(
        DeletePackagesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package.Bulk_delete_packages.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to cancel a package and its associated tracking number.
    /// The <b>packageId</b> value of the package to be canceled must be input as a path parameter in the request.
    /// <b>Note:</b> Packages cannot be canceled if they are currently being processed.
    /// </summary>
    public Task CancelPackageAsync(
        string packageId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package[packageId].Cancel.PostAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to clone a package for redelivery, resending the package with the same information input during the <b>createPackage</b> call.
    /// The <b>packageId</b> value of the package to be cloned must be input as a path parameter in the request.
    /// </summary>
    public Task<ClonePackageResponses?> ClonePackageAsync(
        string packageId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package[packageId].Clone.PostAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to confirm the shipping order for a package.
    /// The <b>packageId</b> value of the package to be confirmed must be input as a path parameter in the request.
    /// After confirmation the estimated cost will be frozen in your wallet and the pickup request will be submitted.
    /// </summary>
    public Task ConfirmPackageAsync(
        string packageId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package[packageId].Confirm.PostAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to create a package for an order.
    /// To create a package, the package's shipping information, such as the dimensions, weight, ship from, and ship to address, as well as information about the item(s) it contains must be specified in the request.
    /// If the call is successful, the <b>packageId</b> and tracking number for the newly created package will be returned.
    /// <b>Note:</b> Sellers should keep track of the returned <b>packageId</b>, as there is no programmatic way to retrieve this value at this time.
    /// </summary>
    public Task<AddPackageResponses?> CreatePackageAsync(
        AddPackageRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve a package based on the provided package ID value.
    /// The <b>packageId</b> value of the package to be retrieved must be input as a path parameter in the request. If the call is successful, information about the package is returned, such as the package's shipping information (its dimensions, weight, ship from, and ship to address) and information about the item(s) it contains.
    /// </summary>
    public Task<GetPackageDetailResponses?> GetPackageAsync(
        string packageId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package[packageId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to delete a package.
    /// The <b>packageId</b> value of the package to be deleted must be input as a path parameter in the request. If the call is successful, HTTP status code <b>204 No Content</b> will be returned, and the package will be deleted.
    /// <b>Note:</b> Packages can only be deleted once they have been cancelled through the <b>cancelPackage</b> or <b>bulkCancelPackages</b> methods and no longer have a tracking number associated with them.
    /// </summary>
    public Task DeletePackageAsync(
        string packageId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package[packageId].DeleteAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve one or more packages based on the provided order line item ID.
    /// This method can be followed up with <see href="/api-docs/sell/edelivery_international_shipping/resources/package/methods/getPackage">getPackage</see> call to retrieve more detailed information about a specific package.
    /// </summary>
    public Task<GetItemPackageIdResponses?> GetPackagesByLineItemIDAsync(
        string orderLineItemId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Package[orderLineItemId].Item.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve metadata on available shipping services, such as the name, description, and directions.
    /// </summary>
    public Task<GetServiceListResponses?> GetServicesAsync(
        string? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Services.GetAsync(cfg =>
        {
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> This method is only available for Greater-China based sellers with an active eDIS account.
    /// This method can be used to retrieve tracking event details for a package associated with the provided tracking number.
    /// Each tracking event is returned in the <b>trackingDetails</b> array and includes the description, location, time, and status of the event.
    /// </summary>
    public Task<GetTrackingDetailResponses?> GetTrackingAsync(
        string trackingNumber,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Tracking.GetAsync(cfg =>
        {
            cfg.QueryParameters.TrackingNumber = trackingNumber;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}

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
using EBay.OtherApis.Generated.VeroApiV2.Models;
using EBay.OtherApis.Generated.VeroApiV2;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

public sealed class VeroApiV2Client
{
    private readonly VeroApiV2ApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal VeroApiV2Client(VeroApiV2ApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> You must be a member of the Verified Rights Owner (VeRO) Program to use this call.
    /// This method is used to retrieve the details of a specific VeRO reason code, including a description of the code and the marketplace associated with the code.
    /// The <b>vero_reason_code_id</b> of the VeRO reason code for which to retrieve details is required as a path parameter.
    /// </summary>
    public Task<VeroReasonCode?> GetVeroReasonCodeAsync(
        string veroReasonCodeId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Vero_reason_code[veroReasonCodeId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> You must be a member of the Verified Rights Owner (VeRO) Program to use this call.
    /// This method retrieves the details of an array of VeRO reason codes, including descriptions of each code and the marketplace associated with each code.
    /// If the <b>X-EBAY-C-MARKETPLACE-ID</b> header is present, only VeRO reason codes for that specific site will be returned. If this header is not included, all reason codes for all sites are returned.
    /// </summary>
    public Task<VeroReasonCodeDetailResponse?> GetVeroReasonCodesAsync(
        string? xEBAYCMARKETPLACEID = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Vero_reason_code.GetAsync(cfg =>
        {
            if (xEBAYCMARKETPLACEID is not null)
            {
                cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> You must be a member of the Verified Rights Owner (VeRO) Program to use this call.
    /// This method can be used to report a violation for an item that allegedly infringes on your copyright, trademark, or other intellectual property rights.
    /// <b>Note:</b> Reporting for shoppable videos will be available in a future release. Live events can only be reported using the VeRO reason code <b>9731</b>.
    /// The request body must include <b>reporterId</b> (the submitting entity's VeRO account identifier), <b>rightsOwnerId</b> (the ID of the Rights Owner from the VeRO Portal), a <b>reportViolations</b> array (with the <b>itemId</b> for each reported item), the <b>veroReasonCodeId</b> of the claimed infringement, and the <b>reportType</b> identifying the kind of content being reported.
    /// You can report one or more items at a time using this method. A maximum of 150 violations can be reported using this method.
    /// </summary>
    public Task<ReportVeroViolationResponse?> ReportVeroViolationsAsync(
        ReportVeroViolationRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Report_vero_violations.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> You must be a member of the Verified Rights Owner (VeRO) Program to use this call.
    /// This method retrieves status information about a VeRO report you have submitted to eBay for copyright, trademark, or other intellectual property rights infringement.
    /// The <b>vero_report_id</b> path parameter is used to specify the eBay report for which to retrieve status information.
    /// </summary>
    public Task<VeroReport?> GetVeroReportAsync(
        string veroReportId,
        bool? includeReportDetails = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Vero_report[veroReportId].GetAsync(cfg =>
        {
            if (includeReportDetails is not null)
            {
                cfg.QueryParameters.IncludeReportDetails = includeReportDetails;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Retrieves status for VeRO infringement reports by item and date range.
    /// </summary>
    public Task<VeroReportStatusResponse?> GetVeroReportItemsAsync(
        string? filter = null,
        string? itemId = null,
        int? limit = null,
        int? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Vero_report_items.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (itemId is not null)
            {
                cfg.QueryParameters.ItemId = itemId;
            }
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
}

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
using EBay.OtherApis.Generated.Vero;
using EBay.OtherApis.Generated.Vero.Models;
using EBay.OtherApis.Exceptions;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

/// <summary>
/// Wrapper for Vero API (commerce_vero).
/// Provides VERO reporting and management operations.
/// </summary>
public sealed class VeroClient
{
    private readonly VeroApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal VeroClient(VeroApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    // ── VERO Reports ──────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a VERO report for intellectual property violations.
    /// </summary>
    public async Task<VeroReportItemsResponse?> CreateVeroReportAsync(VeroReportItemsRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Vero_report.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    /// <summary>
    /// Retrieves a specific VERO report by ID.
    /// </summary>
    public async Task<ReportStatusResponse?> GetVeroReportAsync(string veroReportId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Vero_report[veroReportId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── VERO Reason Codes ─────────────────────────────────────────────────────

    /// <summary>
    /// Retrieves a list of all available VERO reason codes.
    /// </summary>
    public async Task<VeroReasonCodeDetailResponse?> GetVeroReasonCodesAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Vero_reason_code.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    /// <summary>
    /// Retrieves a specific VERO reason code by ID.
    /// </summary>
    public async Task<VeroReasonCodeResponse?> GetVeroReasonCodeAsync(string veroReasonCodeId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Vero_reason_code[veroReasonCodeId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── VERO Report Items ─────────────────────────────────────────────────────

    /// <summary>
    /// Retrieves all items associated with VERO reports.
    /// </summary>
    public async Task<VeroReportStatusResponse?> GetVeroReportItemsAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Vero_report_items.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }
}

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
using EBay.OtherApis.Generated.Negotiation;
using EBay.OtherApis.Generated.Negotiation.Models;
using EBay.OtherApis.Exceptions;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

/// <summary>
/// Wrapper for Negotiation API (sell/negotiation).
/// Provides seller-initiated discount offers to interested buyers.
/// </summary>
public sealed class NegotiationClient
{
    private readonly NegotiationApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal NegotiationClient(NegotiationApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    // ── Eligible Items ────────────────────────────────────────────────────────

    /// <summary>
    /// Finds eligible items where buyers have shown interest and can receive seller-initiated offers.
    /// </summary>
    /// <param name="limit">Maximum number of items to return (1-200). Default: 10</param>
    /// <param name="offset">Number of results to skip for pagination. Default: 0</param>
    /// <param name="accessToken">OAuth token for authentication</param>
    /// <param name="requestOptions">Additional request options (headers, marketplace ID)</param>
    /// <param name="ct">Cancellation token</param>
    public async Task<PagedEligibleItemCollection?> FindEligibleItemsAsync(
        string? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Find_eligible_items.GetAsync(cfg =>
                {
                    if (limit is not null) cfg.QueryParameters.Limit = limit;
                    if (offset is not null) cfg.QueryParameters.Offset = offset;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }

    // ── Send Offers ───────────────────────────────────────────────────────────

    /// <summary>
    /// Sends discount offers to buyers who have shown interest in a listing.
    /// </summary>
    /// <param name="body">Offer details including price/discount, duration, message, and items</param>
    /// <param name="accessToken">OAuth token for authentication</param>
    /// <param name="requestOptions">Additional request options (headers, marketplace ID)</param>
    /// <param name="ct">Cancellation token</param>
    public async Task<SendOfferToInterestedBuyersCollectionResponse?> SendOfferToInterestedBuyersAsync(
        CreateOffersRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Send_offer_to_interested_buyers.PostAsync(body, cfg =>
                    ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }
}

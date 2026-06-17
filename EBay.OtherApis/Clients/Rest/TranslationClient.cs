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
using EBay.OtherApis.Generated.Translation;
using EBay.OtherApis.Generated.Translation.Models;
using EBay.OtherApis.Exceptions;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

/// <summary>
/// Wrapper for Translation API (commerce_translation).
/// Provides text translation operations.
/// </summary>
public sealed class TranslationClient
{
    private readonly TranslationApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal TranslationClient(TranslationApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    // ── Translation ───────────────────────────────────────────────────────────

    /// <summary>
    /// Translates listing title and listing description text from one language into another.
    /// For a full list of supported language translations, see the eBay documentation.
    /// </summary>
    public async Task<TranslateResponse?> TranslateAsync(TranslateRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () =>
                _kiota.Translate.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct));
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex) when (ex.ResponseStatusCode == 401)
        {
            throw new UnauthorizedException(ex);
        }
    }
}

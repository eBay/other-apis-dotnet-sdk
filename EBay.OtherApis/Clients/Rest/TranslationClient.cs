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
using EBay.OtherApis.Generated.Translation.Models;
using EBay.OtherApis.Generated.Translation;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

public sealed class TranslationClient
{
    private readonly TranslationApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal TranslationClient(TranslationApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// This method translates listing title and listing description text from one language into another. For a full list of supported language translations, see the <see href="/develop/guides-v2/other-apis/other-apis-guide#supported-languages">table</see>.
    /// </summary>
    public Task<TranslateResponse?> TranslateAsync(
        TranslateRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Translate.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}

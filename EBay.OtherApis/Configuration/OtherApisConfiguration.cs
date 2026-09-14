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


namespace EBay.OtherApis.Configuration;

public sealed class OtherApisConfiguration
{
    private string _shippingBaseUrl = "https://api.ebay.com/sell/edelivery/international_shipping/v1";
    private string _identityBaseUrl = "https://apiz.ebay.com/commerce/identity/v1";
    private string _translationBaseUrl = "https://apim.ebay.com/commerce/translation/v1_beta";
    private string _veroBaseUrl = "https://api.ebay.com/commerce/vero/v1";

    public string ShippingBaseUrl
    {
        get => _shippingBaseUrl;
        set => _shippingBaseUrl = RequireHttps(value, nameof(ShippingBaseUrl));
    }

    public string IdentityBaseUrl
    {
        get => _identityBaseUrl;
        set => _identityBaseUrl = RequireHttps(value, nameof(IdentityBaseUrl));
    }

    public string TranslationBaseUrl
    {
        get => _translationBaseUrl;
        set => _translationBaseUrl = RequireHttps(value, nameof(TranslationBaseUrl));
    }

    public string VeroBaseUrl
    {
        get => _veroBaseUrl;
        set => _veroBaseUrl = RequireHttps(value, nameof(VeroBaseUrl));
    }

    /// <summary>Fallback bearer token used when no per-call accessToken is supplied.</summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// When true, logs the HTTP method, URL, and status code of non-success responses to stderr.
    /// Response bodies are intentionally excluded to avoid leaking PII.
    /// <para>
    /// <b>WARNING — PII risk.</b> URLs may contain item IDs and query parameters that are
    /// sensitive in context. Never enable this in production. Ensure logs are access-controlled
    /// and not persisted to any external system when enabled in non-production environments.
    /// </para>
    /// </summary>
    public bool DiagnosticLogging { get; set; }

    /// <summary>Timeout applied to all HTTP requests. Defaults to 30 seconds.</summary>
    public TimeSpan HttpTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>Default headers applied to every REST request. Per-call RequestOptions override these.</summary>
    public RequestOptions DefaultRequestOptions { get; set; } = new();

    internal static IEnumerable<string> AllowedHosts =>
    [
        "api.ebay.com",
        "apim.ebay.com",
        "apiz.ebay.com",
    ];

    private static string RequireHttps(string value, string paramName)
    {
        if (!value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only HTTPS endpoints are allowed.", paramName);
        return value;
    }
}

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
    private string _identityBaseUrl = "https://apiz.ebay.com/commerce/identity/v1";
    private string _internationalShippingBaseUrl = "https://api.ebay.com/sell/edelivery_international_shipping/v1";
    private string _translationBaseUrl = "https://api.ebay.com/commerce/translation/v1_beta";
    private string _veroApiV2BaseUrl = "https://api.ebay.com/commerce/vero/v2";

    public string IdentityBaseUrl
    {
        get => _identityBaseUrl;
        set => _identityBaseUrl = RequireHttps(value);
    }

    public string InternationalShippingBaseUrl
    {
        get => _internationalShippingBaseUrl;
        set => _internationalShippingBaseUrl = RequireHttps(value);
    }

    public string TranslationBaseUrl
    {
        get => _translationBaseUrl;
        set => _translationBaseUrl = RequireHttps(value);
    }

    public string VeroApiV2BaseUrl
    {
        get => _veroApiV2BaseUrl;
        set => _veroApiV2BaseUrl = RequireHttps(value);
    }

    /// <summary>Fallback bearer token used when no per-call accessToken is supplied.</summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// When true, logs the HTTP method, URL, status code, and response reason phrase for all
    /// non-success responses to stderr.
    /// <para>
    /// <b>WARNING — PII risk.</b> Response bodies and headers may contain personally identifiable
    /// information. Do not enable in production without appropriate data-handling controls.
    /// </para>
    /// </summary>
    public bool DiagnosticLogging { get; set; }

    /// <summary>Timeout applied to all HTTP requests. Defaults to 30 seconds.</summary>
    public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>Default headers applied to every REST request. Per-call RequestOptions override these.</summary>
    public RequestOptions DefaultRequestOptions { get; set; } = new();

    internal string RestBaseUrl(string apiId) => apiId switch
    {
        "identity" => _identityBaseUrl,
        "international-shipping" => _internationalShippingBaseUrl,
        "translation" => _translationBaseUrl,
        "vero-api-v2" => _veroApiV2BaseUrl,
        _ => throw new ArgumentOutOfRangeException(nameof(apiId), apiId, "Unknown REST API.")
    };

    /// <summary>eBay hosts that receive the access token by default.</summary>
    public static IReadOnlyList<string> DefaultAllowedHosts { get; } = Array.AsReadOnly(new[]
    {
        "api.ebay.com",
        "api.sandbox.ebay.com",
        "apiz.ebay.com",
        "apiz.sandbox.ebay.com"
    });

    /// <summary>
    /// Additional hosts the SDK trusts with the access token, for example <c>api.sandbox.ebay.com</c>.
    /// Every configured base URL must use a host from <see cref="DefaultAllowedHosts"/> or this list;
    /// the client constructor throws otherwise. Requests to any other host never carry the token.
    /// </summary>
    public IList<string> AllowedHosts { get; set; } = new List<string>();

    internal ISet<string> TrustedHosts() =>
        new HashSet<string>(DefaultAllowedHosts.Concat(AllowedHosts ?? new List<string>()), StringComparer.OrdinalIgnoreCase);

    internal bool IsTrustedHost(Uri? uri) =>
        uri is not null
        && string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
        && TrustedHosts().Contains(uri.Host);

    internal void ValidateTrustedHosts()
    {
        var trusted = TrustedHosts();
        foreach (var (name, url) in new (string Name, string Url)[]
        {
            ("IdentityBaseUrl", IdentityBaseUrl),
            ("InternationalShippingBaseUrl", InternationalShippingBaseUrl),
            ("TranslationBaseUrl", TranslationBaseUrl),
            ("VeroApiV2BaseUrl", VeroApiV2BaseUrl)
        })
        {
            var host = new Uri(url).Host;
            if (!trusted.Contains(host))
            {
                throw new InvalidOperationException(
                    $"{name} uses host '{host}', which is not trusted with the access token. Add it to OtherApisConfiguration.AllowedHosts.");
            }
        }
    }

    private static string RequireHttps(string value)
    {
        if (!value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"URL must use HTTPS: '{value}'", nameof(value));
        }
        return value;
    }
}

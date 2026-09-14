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

using EBay.OtherApis.Clients.Rest;
using EBay.OtherApis.Configuration;
using EBay.OtherApis.Generated.Identity;
using EBay.OtherApis.Generated.InternationalShipping;
using EBay.OtherApis.Generated.Translation;
using EBay.OtherApis.Generated.Vero;
using EBay.OtherApis.Generated.VeroApiV2;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.OtherApis;

public class OtherApisClient : IDisposable
{
    internal const string IdentityClientName = "OtherApisClient.Identity";
    internal const string InternationalShippingClientName = "OtherApisClient.InternationalShipping";
    internal const string TranslationClientName = "OtherApisClient.Translation";
    internal const string VeroApiV2ClientName = "OtherApisClient.VeroApiV2";
    internal const string VeroClientName = "OtherApisClient.Vero";

    private readonly HttpClient[] _ownedClients;

    public IdentityClient Identity { get; private set; } = null!;
    public InternationalShippingClient InternationalShipping { get; private set; } = null!;
    public TranslationClient Translation { get; private set; } = null!;
    public VeroApiV2Client VeroApiV2 { get; private set; } = null!;
    public VeroClient Vero { get; private set; } = null!;

    /// <summary>
    /// DI-friendly constructor. Named HttpClients come from <see cref="IHttpClientFactory"/>.
    /// </summary>
    internal OtherApisClient(
        OtherApisConfiguration config,
        IHttpClientFactory factory,
        ITokenContext tokenContext)
    {
        _ownedClients = [];
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, config, tokenContext, endpoint => factory.CreateClient(endpoint.HttpClientName));
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClients with DiagnosticLoggingHandler.
    /// </summary>
    public OtherApisClient(OtherApisConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        InitializeRestClients(auth, config, tokenContext, endpoint =>
        {
            var client = BuildStandaloneHttpClient(config, endpoint.BaseUrl);
            owned.Add(client);
            return client;
        });
        _ownedClients = [.. owned];
    }

    private static HttpClient BuildStandaloneHttpClient(OtherApisConfiguration config, string baseUrl)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = config.HttpTimeout };
    }

    internal static IReadOnlyList<(string ApiId, string HttpClientName)> RestEndpointNames { get; } =
    [
        ("identity", IdentityClientName),
        ("international-shipping", InternationalShippingClientName),
        ("translation", TranslationClientName),
        ("vero-api-v2", VeroApiV2ClientName),
        ("vero", VeroClientName)
    ];

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(OtherApisConfiguration config) =>
        [.. RestEndpointNames.Select(name => new RestEndpoint(name.ApiId, name.HttpClientName, config.RestBaseUrl(name.ApiId)))];

    private void InitializeRestClients(
        IAuthenticationProvider auth,
        OtherApisConfiguration config,
        ITokenContext tokenContext,
        RestHttpClientProvider httpClientFor)
    {
        var endpoints = RestEndpoints(config);
        var identityAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "identity")));
        Identity = new IdentityClient(new IdentityApiClient(identityAdapter), config, tokenContext, identityAdapter);
        var internationalShippingAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "international-shipping")));
        InternationalShipping = new InternationalShippingClient(new InternationalShippingApiClient(internationalShippingAdapter), config, tokenContext, internationalShippingAdapter);
        var translationAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "translation")));
        Translation = new TranslationClient(new TranslationApiClient(translationAdapter), config, tokenContext, translationAdapter);
        var veroApiV2Adapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "vero-api-v2")));
        VeroApiV2 = new VeroApiV2Client(new VeroApiV2ApiClient(veroApiV2Adapter), config, tokenContext, veroApiV2Adapter);
        var veroAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "vero")));
        Vero = new VeroClient(new VeroApiClient(veroAdapter), config, tokenContext, veroAdapter);
    }

    public void Dispose()
    {
        foreach (var client in _ownedClients)
        {
            client.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

internal sealed record RestEndpoint(string ApiId, string HttpClientName, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);

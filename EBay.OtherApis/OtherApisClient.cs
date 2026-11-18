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
using EBay.OtherApis.Generated.VeroApiV2;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.OtherApis;

/// <summary>
/// Entry point for the Other APIs APIs. The client owns its HTTP connections:
/// create one per application (or register it with <c>AddOtherApisSdk</c>, which adds it
/// as a singleton) and dispose it on shutdown. Reach the individual APIs through its properties.
/// </summary>
public class OtherApisClient : IDisposable
{
    private readonly HttpClient[] _ownedClients;

    public IdentityClient Identity { get; private set; } = null!;
    public InternationalShippingClient InternationalShipping { get; private set; } = null!;
    public TranslationClient Translation { get; private set; } = null!;
    public VeroApiV2Client VeroApiV2 { get; private set; } = null!;

    /// <summary>Creates a client with its own HTTP connections.</summary>
    public OtherApisClient(OtherApisConfiguration config)
        : this(config, CreateDefaultPrimaryHandler, disposeHandlers: true)
    {
    }

    /// <summary>
    /// Creates a client that sends every request through <paramref name="primaryHandler"/>, for example to
    /// configure a proxy or client certificates, or to substitute a test double. The handler is shared by
    /// all APIs and is not disposed by this client.
    /// </summary>
    public OtherApisClient(OtherApisConfiguration config, HttpMessageHandler primaryHandler)
        : this(config, RequireHandler(primaryHandler), disposeHandlers: false)
    {
    }

    private OtherApisClient(
        OtherApisConfiguration config,
        Func<HttpMessageHandler> primaryHandler,
        bool disposeHandlers)
    {
        config.ValidateTrustedHosts();
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        HttpClient Build(DelegatingHandler logging, string baseUrl)
        {
            logging.InnerHandler = primaryHandler();
            var client = new HttpClient(logging, disposeHandlers)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = config.HttpTimeout
            };
            owned.Add(client);
            return client;
        }
        InitializeRestClients(auth, config, tokenContext, endpoint => Build(new DiagnosticLoggingHandler(config), endpoint.BaseUrl));
        _ownedClients = [.. owned];
    }

    private static HttpMessageHandler CreateDefaultPrimaryHandler() =>
#if NETFRAMEWORK
        // .NET Framework defaults to 2 connections per host outside ASP.NET.
        new HttpClientHandler { MaxConnectionsPerServer = 64 };
#else
        new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) };
#endif

    private static Func<HttpMessageHandler> RequireHandler(HttpMessageHandler primaryHandler)
    {
#if NETFRAMEWORK
        if (primaryHandler is null)
        {
            throw new ArgumentNullException(nameof(primaryHandler));
        }
#else
        ArgumentNullException.ThrowIfNull(primaryHandler);
#endif
        return () => primaryHandler;
    }

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(OtherApisConfiguration config) =>
    [
        new RestEndpoint("identity", config.RestBaseUrl("identity")),
        new RestEndpoint("international-shipping", config.RestBaseUrl("international-shipping")),
        new RestEndpoint("translation", config.RestBaseUrl("translation")),
        new RestEndpoint("vero-api-v2", config.RestBaseUrl("vero-api-v2"))
    ];

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

internal sealed record RestEndpoint(string ApiId, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);

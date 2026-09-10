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
using EBay.OtherApis.Extensions;
using EBay.OtherApis.Generated.Shipping;
using EBay.OtherApis.Generated.Identity;
using EBay.OtherApis.Generated.Translation;
using EBay.OtherApis.Generated.Vero;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace EBay.OtherApis;

public sealed class OtherApisClient : IDisposable
{
    private readonly StandaloneHttpClientFactory? _standaloneFactory;

    public ShippingClient Shipping { get; }
    public IdentityClient Identity { get; }
    public TranslationClient Translation { get; }
    public VeroClient Vero { get; }

    /// <summary>
    /// DI-friendly constructor. Uses named HttpClients from IHttpClientFactory so that
    /// Polly retry/circuit-breaker/telemetry handlers registered by the application are honoured.
    /// </summary>
    internal OtherApisClient(OtherApisConfiguration config, IHttpClientFactory factory, ITokenContext tokenContext)
    {
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));

        Shipping = new ShippingClient(
            new ShippingApiClient(new HttpClientRequestAdapter(auth,
                httpClient: factory.CreateClient(ServiceCollectionExtensions.ShippingClientName))),
            config, tokenContext);

        Identity = new IdentityClient(
            new IdentityApiClient(new HttpClientRequestAdapter(auth,
                httpClient: factory.CreateClient(ServiceCollectionExtensions.IdentityClientName))),
            config, tokenContext);

        Translation = new TranslationClient(
            new TranslationApiClient(new HttpClientRequestAdapter(auth,
                httpClient: factory.CreateClient(ServiceCollectionExtensions.TranslationClientName))),
            config, tokenContext);

        Vero = new VeroClient(
            new VeroApiClient(new HttpClientRequestAdapter(auth,
                httpClient: factory.CreateClient(ServiceCollectionExtensions.VeroClientName))),
            config, tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Builds its own HttpClients (one per sub-API) with the diagnostic handler chain.
    /// </summary>
    public OtherApisClient(OtherApisConfiguration config)
    {
        _standaloneFactory = new StandaloneHttpClientFactory(config);

        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));

        Shipping = new ShippingClient(
            new ShippingApiClient(new HttpClientRequestAdapter(auth,
                httpClient: _standaloneFactory.CreateClient(ServiceCollectionExtensions.ShippingClientName))),
            config, tokenContext);

        Identity = new IdentityClient(
            new IdentityApiClient(new HttpClientRequestAdapter(auth,
                httpClient: _standaloneFactory.CreateClient(ServiceCollectionExtensions.IdentityClientName))),
            config, tokenContext);

        Translation = new TranslationClient(
            new TranslationApiClient(new HttpClientRequestAdapter(auth,
                httpClient: _standaloneFactory.CreateClient(ServiceCollectionExtensions.TranslationClientName))),
            config, tokenContext);

        Vero = new VeroClient(
            new VeroApiClient(new HttpClientRequestAdapter(auth,
                httpClient: _standaloneFactory.CreateClient(ServiceCollectionExtensions.VeroClientName))),
            config, tokenContext);
    }

    public void Dispose() => _standaloneFactory?.Dispose();

    private sealed class StandaloneHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly OtherApisConfiguration _config;
        private readonly List<HttpClient> _created = [];

        internal StandaloneHttpClientFactory(OtherApisConfiguration config) => _config = config;

        public HttpClient CreateClient(string name)
        {
            var baseUrl = name switch
            {
                ServiceCollectionExtensions.ShippingClientName    => _config.ShippingBaseUrl,
                ServiceCollectionExtensions.IdentityClientName    => _config.IdentityBaseUrl,
                ServiceCollectionExtensions.TranslationClientName => _config.TranslationBaseUrl,
                ServiceCollectionExtensions.VeroClientName        => _config.VeroBaseUrl,
                _ => throw new ArgumentException($"Unknown client name: {name}")
            };

            var handler = new DiagnosticLoggingHandler(_config)
            {
                InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
            };
            var client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = _config.HttpTimeout };
            _created.Add(client);
            return client;
        }

        public void Dispose()
        {
            foreach (var c in _created)
                c.Dispose();
            _created.Clear();
        }
    }
}

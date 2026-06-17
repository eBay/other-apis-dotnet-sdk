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
using Microsoft.Extensions.DependencyInjection;

namespace EBay.OtherApis.Extensions;

public static class ServiceCollectionExtensions
{
    internal const string ShippingClientName = nameof(OtherApisClient) + ".Shipping";
    internal const string IdentityClientName = nameof(OtherApisClient) + ".Identity";
    internal const string TranslationClientName = nameof(OtherApisClient) + ".Translation";
    internal const string VeroClientName = nameof(OtherApisClient) + ".Vero";
    internal const string NegotiationClientName = nameof(OtherApisClient) + ".Negotiation";

    public static IServiceCollection AddOtherApisSdk(
        this IServiceCollection services,
        OtherApisConfiguration config)
    {
        services.AddSingleton(config);
        services.AddSingleton<ITokenContext, AsyncLocalTokenContext>();

        // Register one named HttpClient per sub-API, each with the DiagnosticLoggingHandler
        // and the correct BaseAddress. IHttpClientFactory handles connection pooling.
        services.AddHttpClient(ShippingClientName, (sp, c) =>
                c.BaseAddress = new Uri(sp.GetRequiredService<OtherApisConfiguration>().ShippingBaseUrl))
            .ConfigureHttpClient((sp, c) => c.Timeout = sp.GetRequiredService<OtherApisConfiguration>().HttpTimeout)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<OtherApisConfiguration>()));

        services.AddHttpClient(IdentityClientName, (sp, c) =>
                c.BaseAddress = new Uri(sp.GetRequiredService<OtherApisConfiguration>().IdentityBaseUrl))
            .ConfigureHttpClient((sp, c) => c.Timeout = sp.GetRequiredService<OtherApisConfiguration>().HttpTimeout)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<OtherApisConfiguration>()));

        services.AddHttpClient(TranslationClientName, (sp, c) =>
                c.BaseAddress = new Uri(sp.GetRequiredService<OtherApisConfiguration>().TranslationBaseUrl))
            .ConfigureHttpClient((sp, c) => c.Timeout = sp.GetRequiredService<OtherApisConfiguration>().HttpTimeout)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<OtherApisConfiguration>()));

        services.AddHttpClient(VeroClientName, (sp, c) =>
                c.BaseAddress = new Uri(sp.GetRequiredService<OtherApisConfiguration>().VeroBaseUrl))
            .ConfigureHttpClient((sp, c) => c.Timeout = sp.GetRequiredService<OtherApisConfiguration>().HttpTimeout)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<OtherApisConfiguration>()));

        services.AddHttpClient(NegotiationClientName, (sp, c) =>
                c.BaseAddress = new Uri(sp.GetRequiredService<OtherApisConfiguration>().NegotiationBaseUrl))
            .ConfigureHttpClient((sp, c) => c.Timeout = sp.GetRequiredService<OtherApisConfiguration>().HttpTimeout)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<OtherApisConfiguration>()));

        // Register facade
        services.AddScoped<OtherApisClient>(sp =>
            new OtherApisClient(
                sp.GetRequiredService<OtherApisConfiguration>(),
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<ITokenContext>()));

        // Register sub-clients for direct injection
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Shipping);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Identity);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Translation);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Vero);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Negotiation);

        return services;
    }
}

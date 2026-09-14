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

using EBay.OtherApis;
using EBay.OtherApis.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.OtherApis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOtherApisSdk(
        this IServiceCollection services,
        OtherApisConfiguration config)
    {
        services.AddSingleton(config);
        services.AddSingleton<ITokenContext, AsyncLocalTokenContext>();

        foreach (var endpoint in OtherApisClient.RestEndpointNames)
        {
            var id = endpoint.ApiId;
            services.AddHttpClient(endpoint.HttpClientName, (sp, c) =>
                    c.BaseAddress = new Uri(sp.GetRequiredService<OtherApisConfiguration>().RestBaseUrl(id)))
                .ConfigureHttpClient((sp, c) =>
                    c.Timeout = sp.GetRequiredService<OtherApisConfiguration>().HttpTimeout)
                .AddHttpMessageHandler(sp =>
                    new DiagnosticLoggingHandler(sp.GetRequiredService<OtherApisConfiguration>()));
        }

        services.AddScoped<OtherApisClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new OtherApisClient(
                sp.GetRequiredService<OtherApisConfiguration>(),
                factory,
                sp.GetRequiredService<ITokenContext>());
        });

        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Identity);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().InternationalShipping);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Translation);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().VeroApiV2);
        services.AddScoped(sp => sp.GetRequiredService<OtherApisClient>().Vero);
        return services;
    }
}

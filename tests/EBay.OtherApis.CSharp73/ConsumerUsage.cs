using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EBay.OtherApis;
using EBay.OtherApis.Configuration;
using EBay.OtherApis.Exceptions;

namespace EBay.OtherApis.CSharp73
{
    /// <summary>Typical consumer code written in C# 7.3; it only has to compile.</summary>
    public static class ConsumerUsage
    {
        public static async Task<string> RunAsync()
        {
            var config = new OtherApisConfiguration
            {
                AccessToken = "token",
                HttpTimeout = TimeSpan.FromSeconds(60),
                DiagnosticLogging = true,
                IdentityBaseUrl = "https://ebay-proxy.example.com/commerce/identity/v1",
                AllowedHosts = { "ebay-proxy.example.com" },
                DefaultRequestOptions = new RequestOptions
                {
                    Headers = new Dictionary<string, string> { { "X-EBAY-C-MARKETPLACE-ID", "EBAY_US" } }
                }
            };

            using (var client = new OtherApisClient(config))
            {
                try
                {
                    var result = await client.Identity.GetUserAsync();
                    return result != null ? "ok" : "empty";
                }
                catch (UnauthorizedException)
                {
                    return "unauthorized";
                }
            }
        }
    }
}

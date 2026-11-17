using EBay.OtherApis.Configuration;
using Xunit;

namespace EBay.OtherApis.Tests;

public sealed class TrustedHostsTests
{
    private const string Sandbox = "https://apiz.sandbox.ebay.com/commerce/identity/v1";
    private const string Proxy = "https://ebay-proxy.example.com/commerce/identity/v1";

    [Fact]
    public void Untrusted_base_url_is_rejected_by_both_constructors()
    {
        var config = new OtherApisConfiguration { AccessToken = "t", IdentityBaseUrl = Proxy };

        var standalone = Assert.Throws<InvalidOperationException>(() => new OtherApisClient(config));
        Assert.Contains("AllowedHosts", standalone.Message);
        Assert.Throws<InvalidOperationException>(() => new TestSdk(configure: c => c.IdentityBaseUrl = Proxy));
    }

    [Fact]
    public async Task Production_requests_carry_the_token()
    {
        var sdk = new TestSdk();

        await sdk.Client.Identity.GetUserAsync();

        Assert.StartsWith("https://apiz.ebay.com/commerce/identity/v1/", sdk.Handler.RequestUri!.AbsoluteUri);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Sandbox_hosts_are_trusted_by_default()
    {
        var sdk = new TestSdk(configure: c => c.IdentityBaseUrl = Sandbox);

        await sdk.Client.Identity.GetUserAsync();

        Assert.Equal("apiz.sandbox.ebay.com", sdk.Handler.RequestUri!.Host);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Additional_allowed_host_receives_requests_with_token()
    {
        var sdk = new TestSdk(configure: c =>
        {
            c.IdentityBaseUrl = Proxy;
            c.AllowedHosts.Add("ebay-proxy.example.com");
        });

        await sdk.Client.Identity.GetUserAsync();

        Assert.Equal("ebay-proxy.example.com", sdk.Handler.RequestUri!.Host);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public void Default_hosts_are_read_only()
    {
        Assert.Contains("apiz.ebay.com", OtherApisConfiguration.DefaultAllowedHosts);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)OtherApisConfiguration.DefaultAllowedHosts).Add("evil.example.com"));
    }
}

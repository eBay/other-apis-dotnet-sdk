using EBay.OtherApis.Clients.Rest;
using EBay.OtherApis.Configuration;
using EBay.OtherApis.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EBay.OtherApis.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void Registers_one_shared_entry_client_and_no_individual_api_clients()
    {
        var services = new ServiceCollection();
        services.AddOtherApisSdk(new OtherApisConfiguration { AccessToken = "t" });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<OtherApisClient>();

        Assert.Same(client, provider.CreateScope().ServiceProvider.GetRequiredService<OtherApisClient>());
        Assert.NotNull(client.Identity);
        Assert.Null(provider.GetService<IdentityClient>());
    }

    [Fact]
    public void Rejects_a_null_primary_handler()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new OtherApisClient(new OtherApisConfiguration(), (HttpMessageHandler)null!));
    }
}

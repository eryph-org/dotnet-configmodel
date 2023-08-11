using Eryph.ConfigModel.Networks;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests;

public class ConverterTestBase
{
    protected static void AssertSample1(ProjectNetworksConfig config)
    {
        config.Should().NotBeNull();
        config.Version.Should().Be("1.0");
        config.Project.Should().Be("cinc");
        config.Networks.Should().NotBeNull();
        config.Networks.Should().HaveCount(2);
        config.Networks?[0].Name.Should().Be("default");
        config.Networks?[0].Environment.Should().BeNull();
        config.Networks?[0].Address.Should().Be("192.168.2.0/24");

        config.Networks?[0].Subnets.Should().NotBeNull();
        config.Networks?[0].Subnets.Should().HaveCount(1);
        config.Networks?[0].Subnets?[0].Name.Should().Be("subnet_name");
        config.Networks?[0].Subnets?[0].Address.Should().Be("192.168.2.0/23");
        config.Networks?[0].Subnets?[0].DnsServers.Should().NotBeNull();
        config.Networks?[0].Subnets?[0].DnsServers.Should().HaveCount(2);
        config.Networks?[0].Subnets?[0].Mtu.Should().Be(1300);

        config.Networks?[0].Subnets?[0].IpPools.Should().NotBeNull();
        config.Networks?[0].Subnets?[0].IpPools.Should().HaveCount(1);
        config.Networks?[0].Subnets?[0].IpPools?[0].Name.Should().Be("pool_name");
        config.Networks?[0].Subnets?[0].IpPools?[0].FirstIp.Should().Be("192.168.2.10");
        config.Networks?[0].Subnets?[0].IpPools?[0].LastIp.Should().Be("192.168.2.100");
        
        config.Networks?[0].Provider.Should().NotBeNull();
        config.Networks?[0].Provider?.Name.Should().Be("default");
        config.Networks?[0].Provider?.Subnet.Should().Be("provider_subnet");
        config.Networks?[0].Provider?.IpPool.Should().Be("other_pool");

        config.Networks?[1].Environment.Should().Be("dev");
        config.Networks?[1].Provider.Should().NotBeNull();
        config.Networks?[1].Provider?.Name.Should().Be("default");
        config.Networks?[1].Address.Should().Be("192.168.3.0/24");
    }
}
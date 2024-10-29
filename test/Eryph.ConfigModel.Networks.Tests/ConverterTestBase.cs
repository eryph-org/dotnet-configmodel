using FluentAssertions;

namespace Eryph.ConfigModel.Networks.Tests;

public class ConverterTestBase
{
    protected static void AssertSample1(ProjectNetworksConfig config)
    {
        config.Should().NotBeNull();
        config.Version.Should().Be("1.0");
        config.Project.Should().Be("acme-services");

        config.Networks.Should().SatisfyRespectively(
            network =>
            {
                network.Name.Should().Be("default");
                network.Environment.Should().BeNull();
                network.Address.Should().Be("192.168.2.0/24");

                network.Subnets.Should().SatisfyRespectively(
                    subnet =>
                    {
                        subnet.Name.Should().Be("subnet_name");
                        subnet.Address.Should().Be("192.168.2.0/23");
                        subnet.DnsServers.Should().NotBeNull();
                        subnet.DnsServers.Should().HaveCount(2);
                        subnet.Mtu.Should().Be(1300);
                        subnet.DnsDomain.Should().Be("acme.test");

                        subnet.IpPools.Should().SatisfyRespectively(
                            ipPool =>
                            {
                                ipPool.Name.Should().Be("pool_name");
                                ipPool.FirstIp.Should().Be("192.168.2.10");
                                ipPool.LastIp.Should().Be("192.168.2.100");
                                ipPool.NextIp.Should().Be("192.168.2.20");
                            });
                    });
                
                network.Provider.Should().NotBeNull();
                network.Provider!.Name.Should().Be("default");
                network.Provider!.Subnet.Should().Be("provider_subnet");
                network.Provider!.IpPool.Should().Be("other_pool");
            },
            network =>
            {
                network.Environment.Should().Be("dev");
                
                network.Provider.Should().NotBeNull();
                network.Provider!.Name.Should().Be("default");
                network.Provider.IpPool.Should().BeNull();
                network.Provider.Subnet.Should().BeNull();
                
                network.Address.Should().Be("192.168.3.0/24");
                
                network.Subnets.Should().BeNull();
            });
    }
}

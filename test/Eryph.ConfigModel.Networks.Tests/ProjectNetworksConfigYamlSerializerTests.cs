using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;

namespace Eryph.ConfigModel.Networks.Tests;

public class ProjectNetworksConfigYamlSerializerTests : ProjectNetworksConfigSerializerTestBase
{
    private const string ComplexConfig =
        """
        version: 1.0
        project: acme-services
        networks:
        - name: default
          address: 192.168.2.0/24
          provider:
            name: default
            subnet: provider_subnet
            ip_pool: other_pool
          subnets:
          - name: subnet_name
            address: 192.168.2.0/23
            ip_pools:
            - name: pool_name
              first_ip: 192.168.2.10
              last_ip: 192.168.2.100
              next_ip: 192.168.2.20
            dns_servers:
            - 1.2.3.4
            - 5.6.7.8
            dns_domain: acme.test
            mtu: 1300
        - name: default
          environment: dev
          address: 192.168.3.0/24
          provider:
            name: default
        
        """;

    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_yaml()
    {
        var config = ProjectNetworksConfigYamlSerializer.Deserialize(ComplexConfig);
            
        AssertSample1(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_To_yaml()
    {
        var config = ProjectNetworksConfigYamlSerializer.Deserialize(ComplexConfig);
        var act = ProjectNetworksConfigYamlSerializer.Serialize(config);

        act.Should().Be(ComplexConfig);
    }
}

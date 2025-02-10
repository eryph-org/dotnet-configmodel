using System;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;
using YamlDotNet.Core;

namespace Eryph.ConfigModel.Networks.Tests;

public class ProjectNetworksConfigYamlSerializerTests : ProjectNetworksConfigSerializerTestBase
{
    private const string ComplexConfigYaml =
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
    public void Deserialize_ComplexConfig_ReturnsConfig()
    {
        var config = ProjectNetworksConfigYamlSerializer.Deserialize(ComplexConfigYaml);
            
        AssertComplexConfig(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_ProviderShorthandConfig_ReturnsConfig()
    {
        const string yaml = """
                            networks:
                            - name: default
                              provider: default
                              subnets:
                              - name: subnet_name
                            """;

        var config = ProjectNetworksConfigYamlSerializer.Deserialize(yaml);

        config.Should().NotBeNull();
        config.Networks.Should().SatisfyRespectively(
            network =>
            {
                network.Name.Should().Be("default");

                network.Provider.Should().NotBeNull();
                network.Provider!.Name.Should().Be("default");
                network.Provider.Subnet.Should().BeNull();
                network.Provider.IpPool.Should().BeNull();

                network.Subnets.Should().SatisfyRespectively(
                    subnet => { subnet.Name.Should().Be("subnet_name"); });
            });
    }

    [CulturedFact("en-US")]
    public void Deserialize_InvalidYaml_ThrowsException()
    {
        const string yaml = """
                            networks:
                            - name: ]
                            """;

        var act = () => ProjectNetworksConfigYamlSerializer.Deserialize(yaml);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The YAML is invalid (line 2, column 9):\n"
                         + "While parsing a node, did not find expected node content.\n"
                         + "Make sure to use snake case for names, e.g. 'network_adapters'.")
            .WithInnerException<YamlException>();
    }

    [CulturedFact("en-US")]
    public void Deserialize_UnmappedMember_ThrowsException()
    {
        const string yaml = "unknown_key: test-value";

        var act = () => ProjectNetworksConfigYamlSerializer.Deserialize(yaml);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The YAML is invalid (line 1, column 1):\n"
                         + "Property 'unknown_key' not found on type 'Eryph.ConfigModel.Networks.ProjectNetworksConfig'.\n"
                         + "Make sure to use snake case for names, e.g. 'network_adapters'.")
            .WithInnerException<YamlException>();
    }

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundTrip_ReturnsSameConfig()
    {
        var config = ProjectNetworksConfigYamlSerializer.Deserialize(ComplexConfigYaml);
        var result = ProjectNetworksConfigYamlSerializer.Serialize(config);

        result.Should().Be(ComplexConfigYaml);
    }

    [Fact]
    public void Serialize_SameDataOccursMultipleTimes_ReturnsYamlWithoutAnchors()
    {
        var subnetConfig = new NetworkSubnetConfig
        {
            Name = "subnet",
            Address = "10.0.0.0/24",
        };

        var config = new ProjectNetworksConfig()
        {
            Networks =
            [
                new NetworkConfig
                {
                    Name = "first",
                    Subnets = [subnetConfig],
                },
                new NetworkConfig
                {
                    Name = "second",
                    Subnets = [subnetConfig],
                },
            ],
        };

        var result = ProjectNetworksConfigYamlSerializer.Serialize(config);
        result.Should().NotContain("&").And.NotContain("*");
    }
}

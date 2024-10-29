using System.Collections.Generic;
using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Networks.Tests;

public class ProjectNetworksConfigJsonSerializerTests: ProjectNetworksConfigSerializerTestBase
{
    private const string ComplexConfig =
        """
        {
          "version": "1.0",
          "project": "acme-services",
          "networks": [
            {
              "name": "default",
              "address": "192.168.2.0/24",
              "provider": {
                "name": "default",
                "subnet": "provider_subnet",
                "ipPool": "other_pool"
              },
              "subnets": [
                {
                  "name": "subnet_name",
                  "address": "192.168.2.0/23",
                  "ipPools": [
                    {
                      "name": "pool_name",
                      "firstIp": "192.168.2.10",
                      "lastIp": "192.168.2.100",
                      "nextIp": "192.168.2.20"
                    }
                  ],
                  "dnsServers": [
                    "1.2.3.4",
                    "5.6.7.8"
                  ],
                  "dnsDomain": "acme.test",
                  "mtu": 1300
                }
              ]
            },
            {
              "name": "default",
              "environment": "dev",
              "address": "192.168.3.0/24",
              "provider": {
                "name": "default"
              }
            }
          ]
        }
        """;

    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_json()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(ComplexConfig);
        
        config.Should().NotBeNull();
        AssertSample1(config!);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_to_json()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(ComplexConfig);

        config.Should().NotBeNull();

        var options = new JsonSerializerOptions(ProjectNetworksConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = ProjectNetworksConfigJsonSerializer.Serialize(config!, options);

        result.Should().Be(ComplexConfig);
    }

    [Fact]
    public void Deserialize_JsonRepresentsNull_ThrowsException()
    {
        var act = () => ProjectNetworksConfigJsonSerializer.Deserialize("null");

        act.Should().Throw<JsonException>()
            .WithMessage("The config must not be null.");
    }

    [Fact]
    public void Deserialize_JsonElementRepresentsNull_ThrowsException()
    {
        var element = JsonDocument.Parse("null").RootElement;

        element.Should().NotBeNull();
        element.ValueKind.Should().Be(JsonValueKind.Null);

        var act = () => ProjectNetworksConfigJsonSerializer.Deserialize(element);

        act.Should().Throw<JsonException>()
            .WithMessage("The config must not be null.");
    }
}

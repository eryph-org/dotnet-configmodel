using System;
using System.Collections.Generic;
using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Networks.Tests;

public class ProjectNetworksConfigJsonSerializerTests: ProjectNetworksConfigSerializerTestBase
{
    private const string ComplexConfigJson =
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
                "ip_pool": "other_pool"
              },
              "subnets": [
                {
                  "name": "subnet_name",
                  "address": "192.168.2.0/23",
                  "ip_pools": [
                    {
                      "name": "pool_name",
                      "first_ip": "192.168.2.10",
                      "last_ip": "192.168.2.100",
                      "next_ip": "192.168.2.20"
                    }
                  ],
                  "dns_servers": [
                    "1.2.3.4",
                    "5.6.7.8"
                  ],
                  "dns_domain": "acme.test",
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
    public void Deserialize_ComplexConfig_ReturnsConfig()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(ComplexConfigJson);
        
        config.Should().NotBeNull();
        AssertComplexConfig(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_AfterRoundTripAsJsonElement_ReturnsSameConfig()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(ComplexConfigJson);
        AssertComplexConfig(config);

        var jsonElement = ProjectNetworksConfigJsonSerializer.SerializeToElement(config);
        var result = ProjectNetworksConfigJsonSerializer.Deserialize(jsonElement);

        AssertComplexConfig(result);
    }

    [Fact]
    public void Deserialize_JsonRepresentsNull_ThrowsException()
    {
        var act = () => ProjectNetworksConfigJsonSerializer.Deserialize("null");

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 1, column 1):"
                         + $"{Environment.NewLine}The config must not be null.")
            .WithInnerException<JsonException>();
    }

    [Fact]
    public void Deserialize_JsonElementRepresentsNull_ThrowsException()
    {
        var element = JsonDocument.Parse("null").RootElement;

        element.Should().NotBeNull();
        element.ValueKind.Should().Be(JsonValueKind.Null);

        var act = () => ProjectNetworksConfigJsonSerializer.Deserialize(element);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 1, column 1):"
                         + $"{Environment.NewLine}The config must not be null.")
            .WithInnerException<JsonException>();
    }

    [Fact]
    public void Deserialize_InvalidJson_ThrowsException()
    {
        const string json = """
                            {
                              "test": ]
                            }
                            """;

        var act = () => ProjectNetworksConfigJsonSerializer.Deserialize(json);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 2, column 11):"
                         + $"{Environment.NewLine}']' is an invalid start of a value.*")
            .WithInnerException<JsonException>();
    }

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundTrip_ReturnsSameConfig()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(ComplexConfigJson);

        var options = new JsonSerializerOptions(ProjectNetworksConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = ProjectNetworksConfigJsonSerializer.Serialize(config, options);

        result.Should().Be(ComplexConfigJson);
    }
}

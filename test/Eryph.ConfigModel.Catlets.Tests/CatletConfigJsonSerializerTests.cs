using System;
using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class CatletConfigJsonSerializerTests : CatletConfigSerializerTestBase
{
    private const string ComplexConfigJson =
        """
        {
          "project": "homeland",
          "name": "cinc-windows",
          "location": "cinc",
          "hostname": "cinc-host",
          "environment": "world",
          "store": "home",
          "parent": "dbosoft/winsrv2019-standard/20220324",
          "cpu": {
            "count": 4
          },
          "memory": {
            "startup": 1024,
            "minimum": 512,
            "maximum": 4096
          },
          "drives": [
            {
              "name": "data",
              "mutation": "Overwrite",
              "location": "cinc-shared",
              "store": "ds2",
              "source": "some_template",
              "size": 1,
              "type": "SharedVHD"
            }
          ],
          "network_adapters": [
            {
              "name": "eth0",
              "mac_address": "4711"
            },
            {
              "name": "eth1",
              "mutation": "Remove",
              "mac_address": "4712"
            }
          ],
          "capabilities": [
            {
              "name": "nested_virtualization"
            },
            {
              "name": "secure_boot",
              "mutation": "Remove",
              "details": [
                "tpm",
                "shielded"
              ]
            }
          ],
          "networks": [
            {
              "name": "default",
              "adapter_name": "eth0",
              "subnet_v4": {
                "name": "other",
                "ip_pool": "other_pool"
              },
              "subnet_v6": {
                "name": "other_v6"
              }
            },
            {
              "name": "backup",
              "adapter_name": "eth1"
            }
          ],
          "variables": [
            {
              "name": "first",
              "value": "first value"
            },
            {
              "name": "second",
              "type": "Number",
              "value": "-4.2",
              "secret": true,
              "required": true
            }
          ],
          "fodder": [
            {
              "name": "first"
            },
            {
              "name": "admin-windows",
              "type": "cloud-config",
              "content": "users:\n  - name: Admin\n    groups: [ \u0022Administrators\u0022 ]\n    passwd: \u0022{{password}}\u0022\n",
              "file_name": "filename",
              "secret": true,
              "variables": [
                {
                  "name": "password",
                  "type": "String",
                  "value": "InitialPassw0rd",
                  "secret": true,
                  "required": true
                }
              ]
            }
          ]
        }
        """;

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_ComplexConfig_ReturnsConfig()
    {
        var config = CatletConfigJsonSerializer.Deserialize(ComplexConfigJson);
        
        AssertComplexConfig(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_AfterRoundTripAsJsonElement_ReturnsSameConfig()
    {
        var config = CatletConfigJsonSerializer.Deserialize(ComplexConfigJson);
        AssertComplexConfig(config);

        var jsonElement = CatletConfigJsonSerializer.SerializeToElement(config);
        var result = CatletConfigJsonSerializer.Deserialize(jsonElement);

        AssertComplexConfig(result);
    }

    [Fact]
    public void Deserialize_JsonRepresentsNull_ThrowsException()
    {
        var act = () => CatletConfigJsonSerializer.Deserialize("null");
        
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

        var act = () => CatletConfigJsonSerializer.Deserialize(element);

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

        var act = () => CatletConfigJsonSerializer.Deserialize(json);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 2, column 11):"
                         + $"{Environment.NewLine}']' is an invalid start of a value.*")
            .WithInnerException<JsonException>();
    }

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundTrip_ReturnsSameConfig()
    {
        var config = CatletConfigJsonSerializer.Deserialize(ComplexConfigJson);

        config.Should().NotBeNull();

        var options = new JsonSerializerOptions(CatletConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = CatletConfigJsonSerializer.Serialize(config, options);

        result.Should().Be(ComplexConfigJson);
    }
}

using System.Text.Json;
using System.Text.Json.Nodes;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class CatletConfigJsonSerializerTests : CatletConfigSerializerTestBase
{
    private const string SampleJson1 =
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
          "networkAdapters": [
            {
              "name": "eth0",
              "macAddress": "4711"
            },
            {
              "name": "eth1",
              "mutation": "Remove",
              "macAddress": "4712"
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
              "adapterName": "eth0",
              "subnetV4": {
                "name": "other",
                "ipPool": "other_pool"
              },
              "subnetV6": {
                "name": "other_v6"
              }
            },
            {
              "name": "backup",
              "adapterName": "eth1"
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
              "content": "users:\n  - name: Admin\ngroups: [ \u0022Administrators\u0022 ]\n  passwd: {{password}}",
              "fileName": "filename",
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
    public void Converts_from_json()
    {
        var config = CatletConfigJsonSerializer.Deserialize(SampleJson1);
        
        config.Should().NotBeNull();
        AssertSample1(config!);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_to_json()
    {
        var config = CatletConfigJsonSerializer.Deserialize(SampleJson1);

        config.Should().NotBeNull();

        var options = new JsonSerializerOptions(CatletConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = CatletConfigJsonSerializer.Serialize(config!, options);
        
        result.Should().Be(SampleJson1);
    }

    [Fact]
    public void Deserialize_JsonRepresentsNull_ThrowsException()
    {
        var act = () => CatletConfigJsonSerializer.Deserialize("null");
        
        act.Should().Throw<JsonException>()
            .WithMessage("The config must not be null.");
    }

    [Fact]
    public void Deserialize_JsonElementRepresentsNull_ThrowsException()
    {
        var element = JsonDocument.Parse("null").RootElement;

        element.Should().NotBeNull();
        element.ValueKind.Should().Be(JsonValueKind.Null);

        var act = () => CatletConfigJsonSerializer.Deserialize(element);

        act.Should().Throw<JsonException>()
            .WithMessage("The config must not be null.");
    }
}

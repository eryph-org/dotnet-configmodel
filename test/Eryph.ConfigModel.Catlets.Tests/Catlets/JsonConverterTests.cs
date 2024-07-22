using System.Globalization;
using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class JsonConverterTests : ConverterTestBase
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

    private const string SampleNativeVariableValuesJson =
        """
        {
          "variables": [
            {
              "name": "boolean",
              "value": true
            },
            {
              "name": "number",
              "value": -4.2
            }
          ],
          "fodder": [
            {
              "name": "fodder",
              "variables": [
                {
                  "name": "boolean",
                  "value": true
                },
                {
                  "name": "number",
                  "value": -4.2
                }
              ]
            }
          ]
        }
        """;


    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = CatletConfigDictionaryConverter.Convert(dictionary);
        AssertSample1(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_native_variable_values_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleNativeVariableValuesJson);
        var config = CatletConfigDictionaryConverter.Convert(dictionary);
        AssertNativeVariableValuesSample(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_to_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = CatletConfigDictionaryConverter.Convert(dictionary, false);

        var copyOptions = new JsonSerializerOptions(ConfigModelJsonSerializer.DefaultOptions)
        {
            WriteIndented = true
        };
        var act = ConfigModelJsonSerializer.Serialize(config, copyOptions);
        act.Should().Be(SampleJson1);
    }
}

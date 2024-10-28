using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public class JsonConverterTests : ConverterTestBase
{
    private const string SampleJson1 =
        """
        {
          "name": "fodder1",
          "variables": [
            {
              "name": "first",
              "value": "first value"
            },
            {
              "name": "second",
              "type": "Boolean",
              "value": "true",
              "secret": true,
              "required": true
            }
          ],
          "fodder": [
            {
              "name": "admin-windows",
              "type": "cloud-config",
              "content": "users:\n  - name: Admin\ngroups: [ \u0022Administrators\u0022 ]\n  passwd: InitialPassw0rd",
              "fileName": "filename",
              "secret": true
            },
            {
              "name": "super-dupa",
              "type": "cloud-config"
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
              "name": "fodder"
            }
          ]
        }
        """;

    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = FodderGeneConfigDictionaryConverter.Convert(dictionary);
        AssertSample1(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_native_variable_values_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleNativeVariableValuesJson);
        var config = FodderGeneConfigDictionaryConverter.Convert(dictionary);
        AssertNativeVariableValuesSample(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_to_json()
    {
        var config = FodderGeneConfigJsonSerializer.Deserialize(SampleJson1);

        config.Should().NotBeNull();

        var options = new JsonSerializerOptions(FodderGeneConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = ConfigModelJsonSerializer.Serialize(config, options);

        result.Should().Be(SampleJson1);
    }
}

using System.Text.Json;
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

    [Fact]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = FodderGeneConfigDictionaryConverter.Convert(dictionary);
        AssertSample1(config);
    }

    [Theory]
    [InlineData(SampleJson1, SampleJson1)]
    public void Converts_to_json(string input, string expected)
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(input);
        var config = FodderGeneConfigDictionaryConverter.Convert(dictionary, false);

        var copyOptions = new JsonSerializerOptions(ConfigModelJsonSerializer.DefaultOptions)
        {
            WriteIndented = true
        };
        var act = ConfigModelJsonSerializer.Serialize(config, copyOptions);
        act.Should().Be(expected);
    }
}

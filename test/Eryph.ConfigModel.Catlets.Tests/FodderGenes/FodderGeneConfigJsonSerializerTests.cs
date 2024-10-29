using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public class FodderGeneConfigJsonSerializerTests : FodderGeneConfigSerializerTestBase
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

    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_json()
    {
        var config = FodderGeneConfigJsonSerializer.Deserialize(SampleJson1);

        config.Should().NotBeNull();
        AssertComplexConfig(config!);
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
        var result = FodderGeneConfigJsonSerializer.Serialize(config!, options);

        result.Should().Be(SampleJson1);
    }

    [Fact]
    public void Deserialize_JsonRepresentsNull_ThrowsException()
    {
        var act = () => FodderGeneConfigJsonSerializer.Deserialize("null");

        act.Should().Throw<JsonException>()
            .WithMessage("The config must not be null.");
    }

    [Fact]
    public void Deserialize_JsonElementRepresentsNull_ThrowsException()
    {
        var element = JsonDocument.Parse("null").RootElement;

        element.Should().NotBeNull();
        element.ValueKind.Should().Be(JsonValueKind.Null);

        var act = () => FodderGeneConfigJsonSerializer.Deserialize(element);

        act.Should().Throw<JsonException>()
            .WithMessage("The config must not be null.");
    }
}

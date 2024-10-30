using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class FodderGeneConfigJsonSerializerTests : FodderGeneConfigSerializerTestBase
{
    private const string ComplexConfigJson =
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
              "remove": true,
              "type": "cloud-config",
              "content": "users:\n  - name: Admin\ngroups: [ \u0022Administrators\u0022 ]\n  passwd: InitialPassw0rd",
              "file_name": "filename",
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
    public void Deserialize_ComplexConfig_ReturnsConfig()
    {
        var config = FodderGeneConfigJsonSerializer.Deserialize(ComplexConfigJson);

        config.Should().NotBeNull();
        AssertComplexConfig(config!);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_AfterRoundTripAsJsonElement_ReturnsSameConfig()
    {
        var config = FodderGeneConfigJsonSerializer.Deserialize(ComplexConfigJson);
        AssertComplexConfig(config);

        var jsonElement = FodderGeneConfigJsonSerializer.SerializeToElement(config);
        var result = FodderGeneConfigJsonSerializer.Deserialize(jsonElement);

        AssertComplexConfig(result);
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

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundtrip_ReturnsSameConfig()
    {
        var config = FodderGeneConfigJsonSerializer.Deserialize(ComplexConfigJson);

        var options = new JsonSerializerOptions(FodderGeneConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = FodderGeneConfigJsonSerializer.Serialize(config, options);

        result.Should().Be(ComplexConfigJson);
    }
}

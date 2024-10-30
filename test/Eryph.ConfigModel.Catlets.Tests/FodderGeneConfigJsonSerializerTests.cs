using System;
using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;

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
              "filename": "filename",
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
        AssertComplexConfig(config);
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

    [CulturedFact("en-US")]
    public void Deserialize_JsonRepresentsNull_ThrowsException()
    {
        var act = () => FodderGeneConfigJsonSerializer.Deserialize("null");

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 1, column 1):"
                         + $"{Environment.NewLine}The config must not be null.")
            .WithInnerException<JsonException>();
    }

    [CulturedFact("en-US")]
    public void Deserialize_JsonElementRepresentsNull_ThrowsException()
    {
        var element = JsonDocument.Parse("null").RootElement;

        element.Should().NotBeNull();
        element.ValueKind.Should().Be(JsonValueKind.Null);

        var act = () => FodderGeneConfigJsonSerializer.Deserialize(element);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 1, column 1):"
                         + $"{Environment.NewLine}The config must not be null.")
            .WithInnerException<JsonException>();
    }

    [CulturedFact("en-US")]
    public void Deserialize_InvalidJson_ThrowsException()
    {
        const string json = """
                            {
                              "fodder": ]
                            }
                            """;

        var act = () => FodderGeneConfigJsonSerializer.Deserialize(json);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 2, column 13):"
                         + $"{Environment.NewLine}']' is an invalid start of a value.*")
            .WithInnerException<JsonException>();
    }

    [CulturedFact("en-US")]
    public void Deserialize_UnmappedMember_ThrowsException()
    {
        const string json = """
                            {
                              "unknown_key": "test-value"
                            }
                            """;

        var act = () => FodderGeneConfigJsonSerializer.Deserialize(json);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The JSON is invalid (line 2, column 17):"
                         + $"{Environment.NewLine}The JSON property 'unknown_key' could not be mapped to any .NET member contained in type 'Eryph.ConfigModel.FodderGenes.FodderGeneConfig'.")
            .WithInnerException<JsonException>();
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

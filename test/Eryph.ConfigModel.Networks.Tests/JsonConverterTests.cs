using System.Collections.Generic;
using System.Text.Json;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using FluentAssertions;

namespace Eryph.ConfigModel.Networks.Tests;

public class JsonConverterTests: ConverterTestBase
{
    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_json()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(Samples.Json1);
        
        config.Should().NotBeNull();
        AssertSample1(config!);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_to_json()
    {
        var config = ProjectNetworksConfigJsonSerializer.Deserialize(Samples.Json1);

        config.Should().NotBeNull();

        var options = new JsonSerializerOptions(ProjectNetworksConfigJsonSerializer.Options)
        {
            WriteIndented = true
        };
        var result = ProjectNetworksConfigJsonSerializer.Serialize(config!, options);

        result.Should().Be(Samples.Json1);
    }
}

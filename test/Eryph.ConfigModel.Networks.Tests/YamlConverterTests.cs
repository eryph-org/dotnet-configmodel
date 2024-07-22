using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;

namespace Eryph.ConfigModel.Networks.Tests;

public class YamlConverterTests : ConverterTestBase
{
    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_yaml()
    {
        var config = ProjectNetworkConfigYamlSerializer.Deserialize(Samples.Yaml1);
            
        AssertSample1(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_To_yaml()
    {
        var config = ProjectNetworkConfigYamlSerializer.Deserialize(Samples.Yaml1);
        var act = ProjectNetworkConfigYamlSerializer.Serialize(config);

        act.Should().Be(Samples.Yaml1);
    }
}

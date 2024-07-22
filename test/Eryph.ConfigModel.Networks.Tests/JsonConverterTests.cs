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
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(Samples.Json1) ??
                         new Dictionary<object, object>();
        var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, false);
        
        AssertSample1(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_to_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(Samples.Json1) ??
                       new Dictionary<object, object>();
        var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, false);
        
        var copyOptions = new JsonSerializerOptions(ConfigModelJsonSerializer.DefaultOptions)
        {
            WriteIndented = true
        };
        
        var act = ConfigModelJsonSerializer.Serialize(config,copyOptions );
        
        act.Should().Be(Samples.Json1);
    }
}

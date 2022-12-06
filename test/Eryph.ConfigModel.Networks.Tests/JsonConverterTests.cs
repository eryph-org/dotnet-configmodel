using System.Text.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class JsonConverterTests: ConverterTestBase
{
    
    [Fact]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(Samples.Json1);
        var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, false);
        AssertSample1(config);
  
    }

    [Theory]
    [InlineData(Samples.Json1, Samples.Json1)]
    public void Converts_to_json(string input, string expected)
    {
      var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(input);
      var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, false);

      var copyOptions = new JsonSerializerOptions(ConfigModelJsonSerializer.DefaultOptions)
      {
        WriteIndented = true
      };
      var act = ConfigModelJsonSerializer.Serialize(config,copyOptions );

      act.Should().Be(expected);

    }
}
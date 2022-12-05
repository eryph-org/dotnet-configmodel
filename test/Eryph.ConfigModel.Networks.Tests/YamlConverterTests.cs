using System.Collections.Generic;
using System.Text.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Catlet.Tests
{
    public class YamlConverterTests : ConverterTestBase
    {
        
        [Fact]
        public void Converts_from_yaml()
        {
          var serializer = new DeserializerBuilder()
              .Build();
          
            var dictionary = serializer.Deserialize<Dictionary<object, object>>(Samples.Yaml1);
            var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, true);
            AssertSample1(config);
        }

        [Fact]
        public void Converts_To_yaml()
        {
          
          // we always convert over json to yaml, so use json as input
          var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(Samples.Json1);

          var serializer = new SerializerBuilder()
              .Build();

          var act = serializer.Serialize(dictionary);

          act.Should().Be(Samples.Yaml1);

        }
    }
}
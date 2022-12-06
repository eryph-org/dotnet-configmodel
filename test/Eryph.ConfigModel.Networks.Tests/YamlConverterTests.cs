using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;


namespace Eryph.ConfigModel.Catlet.Tests
{
    public class YamlConverterTests : ConverterTestBase
    {
        
        [Fact]
        public void Converts_from_yaml()
        {
            var config = ProjectNetworkConfigYamlSerializer.Deserialize(Samples.Yaml1);
            AssertSample1(config);
        }

        [Theory()]
        [InlineData(Samples.Yaml1, Samples.Yaml1)]
        public void Converts_To_yaml(string input, string expected)
        {
            
            var config = ProjectNetworkConfigYamlSerializer.Deserialize(input);
            var act = ProjectNetworkConfigYamlSerializer.Serialize(config);

            act.Should().Be(expected);

        }
    }
}
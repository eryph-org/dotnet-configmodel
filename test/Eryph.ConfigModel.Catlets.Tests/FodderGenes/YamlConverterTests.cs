using System.Collections.Generic;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes
{
    public class YamlConverterTests : ConverterTestBase
    {

        private const string SampleYaml1 = @"name: fodder1
fodder:
- name: admin-windows
  type: cloud-config
  content: >-
    users:
      - name: Admin
        groups: [ ""Administrators"" ]
        passwd: InitialPassw0rd
  file_name: filename
  secret: true
- name: super-dupa
  type: cloud-config
";


        [Fact]
        public void Converts_from_yaml()
        {
            var serializer = new DeserializerBuilder()
              .Build();

            var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleYaml1);
            var config = FodderConfigDictionaryConverter.Convert(dictionary, true);
            AssertSample1(config);
        }

        [Theory()]
        [InlineData(SampleYaml1, SampleYaml1)]
        public void Converts_To_yaml(string input, string expected)
        {
            var config = FodderGeneConfigYamlSerializer.Deserialize(input);
            var act = FodderGeneConfigYamlSerializer.Serialize(config);
            act.Should().Be(expected);

        }

    }
}
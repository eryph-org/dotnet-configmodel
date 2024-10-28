using System.Collections.Generic;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public class YamlConverterTests : ConverterTestBase
{
    private const string SampleYaml1 = 
        """
        name: fodder1
        variables:
        - name: first
          value: first value
        - name: second
          type: Boolean
          value: true
          secret: true
          required: true
        fodder:
        - name: admin-windows
          type: cloud-config
          content: |-
            users:
              - name: Admin
                groups: [ "Administrators" ]
                passwd: InitialPassw0rd
          file_name: filename
          secret: true
        - name: super-dupa
          type: cloud-config
        
        """;

    private const string SampleYaml2 =
        """
        name: fodder1
        variables:
        - name: first
          value: first value
        - name: second
          type: Boolean
          value: true
          secret: true
          required: true
        fodder:
        - name: admin-windows
          type: cloud-config
          content:
            users:
              - name: Admin
                groups: [ "Administrators" ]
                passwd: InitialPassw0rd
          file_name: filename
          secret: true
        - name: super-dupa
          type: cloud-config
        """;

    private const string SampleNativeVariableValuesYaml =
        """
        variables:
        - name: boolean
          value: true
        - name: number
          value: -4.2
        fodder:
        - name: fodder
        
        """;

    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_yaml()
    {
        var serializer = new DeserializerBuilder()
            .Build();

        var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleYaml1);
        var config = FodderGeneConfigDictionaryConverter.Convert(dictionary, true);
        AssertSample1(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_native_variable_values_from_yaml()
    {
        var serializer = new DeserializerBuilder()
            .Build();

        var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleNativeVariableValuesYaml);
        var config = FodderGeneConfigDictionaryConverter.Convert(dictionary);
        AssertNativeVariableValuesSample(config);
    }

    
    [CulturedFact("en-US", "de-DE")]
    public void Converts_To_yaml()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleYaml1);
        var act = FodderGeneConfigYamlSerializer.Serialize(config);
        act.Should().Be(SampleYaml1);
    }

    /*
    [Fact]
    public void Fact()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleYaml2);
        var yaml = FodderGeneConfigYamlSerializer.Serialize(config);
        yaml.Should().Be(SampleYaml2);
    }
    */
}

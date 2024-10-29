using System.Collections.Generic;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public class FodderGeneConfigYamlSerializerTests : FodderGeneConfigSerializerTestBase
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
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleYaml1);

        AssertComplexConfig(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_native_variable_values_from_yaml()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleNativeVariableValuesYaml);
        
        AssertNativeVariableValuesSample(config);
    }
    
    [CulturedFact("en-US", "de-DE")]
    public void Converts_To_yaml()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleYaml1);
        var act = FodderGeneConfigYamlSerializer.Serialize(config);
        act.Should().Be(SampleYaml1);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundtrip_ReturnsSameConfig()
    {
        var yaml = FodderGeneConfigYamlSerializer.Serialize(ComplexConfig);
        var result = FodderGeneConfigYamlSerializer.Deserialize(yaml);

        var yaml2 = FodderGeneConfigYamlSerializer.Serialize(result);

        result.Should().BeEquivalentTo(ComplexConfig);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_ConfigWithNativeFodderContent_ReturnsConfig()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleYaml2);

        AssertComplexConfig(config);
    }

    
    [Fact]
    public void Fact()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(SampleYaml2);
        var yaml = FodderGeneConfigYamlSerializer.Serialize(config);
        yaml.Should().Be(SampleYaml2);
    }
}

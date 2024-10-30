using System.Collections.Generic;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;
using YamlDotNet.Core;

namespace Eryph.ConfigModel.Catlet.Tests;

public class FodderGeneConfigYamlSerializerTests : FodderGeneConfigSerializerTestBase
{
    private const string ComplexConfigYaml =
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
          remove: true
          type: cloud-config
          content: |
            users:
              - name: Admin
                groups: [ "Administrators" ]
                passwd: InitialPassw0rd
          file_name: filename
          secret: true
        - name: super-dupa
          type: cloud-config
        
        """;

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_ComplexConfig_ReturnsConfig()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(ComplexConfigYaml);

        AssertComplexConfig(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_ConfigWithNativeVariableValues_ReturnsConfig()
    {
        const string yaml = """
                            variables:
                            - name: boolean
                              value: true
                            - name: number
                              value: -4.2
                            fodder:
                            - name: fodder
                              variables:
                              - name: boolean
                                value: true
                              - name: number
                                value: -4.2
                            """;

        var config = FodderGeneConfigYamlSerializer.Deserialize(yaml);

        config.Fodder.Should().SatisfyRespectively(
            fodder => fodder.Variables.Should().SatisfyRespectively(
                variable =>
                {
                    variable.Name.Should().Be("boolean");
                    variable.Type.Should().BeNull();
                    variable.Value.Should().Be("true");
                    variable.Required.Should().BeNull();
                    variable.Secret.Should().BeNull();
                },
                variable =>
                {
                    variable.Name.Should().Be("number");
                    variable.Type.Should().BeNull();
                    variable.Value.Should().Be("-4.2");
                    variable.Required.Should().BeNull();
                    variable.Secret.Should().BeNull();
                }));

        config.Variables.Should().SatisfyRespectively(
            variable =>
            {
                variable.Name.Should().Be("boolean");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("true");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            },
            variable =>
            {
                variable.Name.Should().Be("number");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("-4.2");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            });
    }

    [Fact]
    public void Deserialize_FodderContentIsFlowStyleMapping_ThrowsException()
    {
        const string yaml = """
                            fodder:
                            - name: fodder
                              content: { first_key: first_value, second_key: second_value }
                            """;

        var act = () => FodderGeneConfigYamlSerializer.Deserialize(yaml);
        act.Should().Throw<YamlException>()
            .WithMessage("Only indentation style mappings are supported at this point.");
    }

    [Fact]
    public void Deserialize_FodderContentIsFlowStyleSequence_ThrowsException()
    {
        const string yaml = """
                            fodder:
                            - name: fodder
                              content: [ "first_value", "second_task" ]
                            """;

        var act = () => FodderGeneConfigYamlSerializer.Deserialize(yaml);
        act.Should().Throw<YamlException>()
            .WithMessage("Only indentation style sequences are supported at this point.");
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_FodderContentIsIndentationStyleMapping_ReturnsConfig()
    {
        const string yaml = """
                            fodder:
                            - name: first-food
                              content:
                                first_key: first-food-first-value
                                second_key: first-food-second-value
                              secret: true
                            - name : second-food
                              content:
                                first_key: second-food-first-value
                                second_key: second-food-second-value
                            - name : third-food
                              content:
                                first_key: third-food-first-value
                                second_key: third-food-second-value
                            """;

        var config = FodderGeneConfigYamlSerializer.Deserialize(yaml);

        config.Fodder.Should().SatisfyRespectively(
            fodder =>
            {
                fodder.Name.Should().Be("first-food");
                fodder.Content.Should().Be("first_key: first-food-first-value\nsecond_key: first-food-second-value\n");
                fodder.Secret.Should().BeTrue();
            },
            fodder =>
            {
                fodder.Name.Should().Be("second-food");
                fodder.Content.Should().Be("first_key: second-food-first-value\nsecond_key: second-food-second-value\n");
            },
            fodder =>
            {
                fodder.Name.Should().Be("third-food");
                fodder.Content.Should().Be("first_key: third-food-first-value\nsecond_key: third-food-second-value\n");
            });
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_FodderContentIsIndentationStyleSequence_ReturnsConfig()
    {
        const string yaml = """
                            fodder:
                            - name: first-food
                              content:
                                - first-food-first-value
                                - first-food-second-value
                              secret: true
                            - name : second-food
                              content:
                                - second-food-first-value
                                - second-food-second-value
                            - name : third-food
                              content:
                              - third-food-first-value
                              - third-food-second-value
                            """;

        var config = FodderGeneConfigYamlSerializer.Deserialize(yaml);

        config.Fodder.Should().SatisfyRespectively(
            fodder =>
            {
                fodder.Name.Should().Be("first-food");
                fodder.Content.Should().Be("- first-food-first-value\n- first-food-second-value\n");
                fodder.Secret.Should().BeTrue();
            },
            fodder =>
            {
                fodder.Name.Should().Be("second-food");
                fodder.Content.Should().Be("- second-food-first-value\n- second-food-second-value\n");
            },
            fodder =>
            {
                fodder.Name.Should().Be("third-food");
                fodder.Content.Should().Be("- third-food-first-value\n- third-food-second-value\n");
            });
    }

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundtrip_ReturnsSameConfig()
    {
        var config = FodderGeneConfigYamlSerializer.Deserialize(ComplexConfigYaml);
        var result = FodderGeneConfigYamlSerializer.Serialize(config);

        result.Should().BeEquivalentTo(ComplexConfigYaml);
    }
}
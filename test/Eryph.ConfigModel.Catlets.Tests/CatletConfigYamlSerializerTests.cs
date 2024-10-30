using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using System;
using Xunit;
using YamlDotNet.Core;

namespace Eryph.ConfigModel.Catlet.Tests;

public class CatletConfigYamlSerializerTests : CatletConfigSerializerTestBase
{
    private const string ComplexConfigYaml =
        """
        project: homeland
        name: cinc-windows
        location: cinc
        hostname: cinc-host
        environment: world
        store: home
        parent: dbosoft/winsrv2019-standard/20220324
        cpu:
          count: 4
        memory:
          startup: 1024
          minimum: 512
          maximum: 4096
        drives:
        - name: data
          mutation: Overwrite
          location: cinc-shared
          store: ds2
          source: some_template
          size: 1
          type: SharedVHD
        network_adapters:
        - name: eth0
          mac_address: 4711
        - name: eth1
          mutation: Remove
          mac_address: 4712
        capabilities:
        - name: nested_virtualization
        - name: secure_boot
          mutation: Remove
          details:
          - tpm
          - shielded
        networks:
        - name: default
          adapter_name: eth0
          subnet_v4:
            name: other
            ip_pool: other_pool
          subnet_v6:
            name: otherv6
        - name: backup
          adapter_name: eth1
        variables:
        - name: first
          value: first value
        - name: second
          type: Number
          value: -4.2
          secret: true
          required: true
        fodder:
        - name: first
        - name: admin-windows
          type: cloud-config
          content: |
            users:
              - name: Admin
                groups: [ "Administrators" ]
                passwd: "{{password}}"
          file_name: filename
          secret: true
          variables:
          - name: password
            type: String
            value: InitialPassw0rd
            secret: true
            required: true

        """;
    
    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_ComplexConfig_ReturnsConfig()
    {
        var config = CatletConfigYamlSerializer.Deserialize(ComplexConfigYaml);
        
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

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

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

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_MinimalConfig_ReturnsConfig()
    {
        const string yaml = "parent: acme/acme-os/1.0";
        
        var config = CatletConfigYamlSerializer.Deserialize(yaml);

        config.Should().NotBeNull();
        config.Parent.Should().Be("acme/acme-os/1.0");
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_CapabilitiesShorthandConfig_ReturnsConfig()
    {
        const string yaml = """
                            capabilities:
                            - nested_virtualization
                            - secure_boot
                            """;

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

        config.Should().NotBeNull();
        config.Capabilities.Should().SatisfyRespectively(
            capability =>
            {
                capability.Name.Should().Be("nested_virtualization");
                capability.Details.Should().BeNull();
                capability.Mutation.Should().BeNull();
            },
            capability =>
            {
                capability.Name.Should().Be("secure_boot");
                capability.Details.Should().BeNull();
                capability.Mutation.Should().BeNull();
            });
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_CpuShorthandConfig_ReturnsConfig()
    {
        const string yaml = "cpu: 4";

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

        config.Should().NotBeNull();
        config.Cpu.Should().NotBeNull();
        config.Cpu!.Count.Should().Be(4);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Deserialize_MemoryShorthandConfig_ReturnsConfig()
    {
        const string yaml = "memory: 512";
        
        var config = CatletConfigYamlSerializer.Deserialize(yaml);

        config.Should().NotBeNull();
        config.Memory.Should().NotBeNull();
        config.Memory!.Startup.Should().Be(512);
        config.Memory.Minimum.Should().BeNull();
        config.Memory.Maximum.Should().BeNull();
    }

    [Fact]
    public void Deserialize_FodderContentIsFlowStyleMapping_ThrowsException()
    {
        const string yaml = """
                            fodder:
                            - name: fodder
                              content: { first_key: first_value, second_key: second_value }
                            """;

        var act = () => CatletConfigYamlSerializer.Deserialize(yaml);
        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The YAML is invalid (line 3, column 12):"
                         + $"{Environment.NewLine}Only indentation style mappings are supported at this point."
                         + $"{Environment.NewLine}Make sure to use snake case for names, e.g. 'network_adapters'.")
            .WithInnerException<YamlException>();
    }

    [Fact]
    public void Deserialize_FodderContentIsFlowStyleSequence_ThrowsException()
    {
        const string yaml = """
                            fodder:
                            - name: fodder
                              content: [ "first_value", "second_task" ]
                            """;

        var act = () => CatletConfigYamlSerializer.Deserialize(yaml);
        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The YAML is invalid (line 3, column 12):"
                         + $"{Environment.NewLine}Only indentation style sequences are supported at this point."
                         + $"{Environment.NewLine}Make sure to use snake case for names, e.g. 'network_adapters'.")
            .WithInnerException<YamlException>();
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

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

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

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

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

    [Fact]
    public void Deserialize_InvalidYaml_ThrowsException()
    {
        const string yaml = """
                            fodder:
                            - name: ]
                            """;

        var act = () => CatletConfigYamlSerializer.Deserialize(yaml);

        act.Should().Throw<InvalidConfigException>()
            .WithMessage("The YAML is invalid (line 2, column 9):"
                         + $"{Environment.NewLine}While parsing a node, did not find expected node content."
                         + $"{Environment.NewLine}Make sure to use snake case for names, e.g. 'network_adapters'.")
            .WithInnerException<YamlException>();
    }

    [CulturedFact("en-US", "de-DE")]
    public void Serialize_AfterRoundTrip_ReturnsSameConfig()
    {
        var config = CatletConfigYamlSerializer.Deserialize(ComplexConfigYaml);
        var result = CatletConfigYamlSerializer.Serialize(config);

        result.Should().Be(ComplexConfigYaml);
    }
}

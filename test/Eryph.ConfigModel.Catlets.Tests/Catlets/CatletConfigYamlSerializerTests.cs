using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CultureAwareTesting.xUnit;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class CatletConfigYamlSerializerTests : CatletConfigSerializerTestBase
{
    private const string SampleYaml1 =
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
          content: |-
            users:
              - name: Admin
                groups: [ "Administrators" ]
                passwd: "{{password}}"
          file_name: filename
          secret: true
          variables:
          - name: first
            type: String
            value: InitialPassw0rd
            secret: true
            required: true

        """;

    private const string SampleYaml2 = "parent: dbosoft/winsrv2019-standard/20220324";

    private const string SampleYaml3 =
        """
        parent: dbosoft/winsrv2019-standard/20220324  
        cpu: 4
        """;

    private const string SampleYaml4 = 
        """
        capabilities:
        - nested_virtualization                         
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
          variables:
          - name: boolean
            value: true
          - name: number
            value: -4.2
        """;

    private const string FodderContentIndentationStyleObjectCatletYaml =
        """
        fodder:
        - name: fodder
          content:
            users:
            - name: Admin
              groups: [ "Administrators" ]
              passwd: '{{password}}'
        """;

    private const string FodderContentIndentationStyleObjectYaml =
        """
        users:
        - name: Admin
          groups: [ "Administrators" ]
          passwd: '{{password}}'
        """;

    private const string FodderContentFlowStyleObjectCatletYaml =
        """
        fodder:
        - name: fodder
          content: { users: [ { name: Admin, groups: ["Administrators"], passwd: '{{ passwd }}' } ] }
        """;

    private const string FodderContentFlowStyleObjectYaml =
        """
        { users: [ { name: Admin, groups: ["Administrators"], passwd: '{{ passwd }}' } ] }
        """;

    private const string FodderContentIndentationStyleSequenceCatletYaml =
        """
        fodder:
        - name: fodder
          content:
          - name: first_task
          - name: second_task
        """;

    private const string FodderContentIndentationStyleSequenceYaml =
        """
        - name: first_task
        - name: second_task
        """;

    private const string FodderContentFlowStyleSequenceCatletYaml =
        """
        fodder:
        - name: fodder
          content: [ { name: "first_task" }, { name: "second_task" } ]
        """;

    private const string FodderContentFlowStyleSequenceYaml =
        """
        [ { name: "first_task" }, { name: "second_task" } ]
        """;

    private const string MemoryShorthandSampleYaml = "memory: 512";

    [CulturedFact("en-US", "de-DE")]
    public void Converts_from_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(SampleYaml1);
        
        AssertComplexConfig(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_native_variable_values_from_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(SampleNativeVariableValuesYaml);
        
        AssertNativeVariableValuesSample(config);
    }

    [CulturedFact("en-US", "de-DE")]
    public void Converts_To_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(SampleYaml1);
        var act = CatletConfigYamlSerializer.Serialize(config);
        act.Should().Be(SampleYaml1);

    }

    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_minimal_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(SampleYaml2);

        config.Should().NotBeNull();
        config.Parent.Should().Be("dbosoft/winsrv2019-standard/20220324");

    }

    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_short_cpu_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(SampleYaml3);

        config.Should().NotBeNull();
        config.Cpu.Should().NotBeNull();
        config.Cpu?.Count.Should().Be(4);

    }

    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_shorthand_memory_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(MemoryShorthandSampleYaml);

        config.Should().NotBeNull();
        config.Memory.Should().NotBeNull();
        config.Memory!.Startup.Should().Be(512);
        config.Memory.Minimum.Should().BeNull();
        config.Memory.Maximum.Should().BeNull();
    }

    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_short_features_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(SampleYaml4);

        config.Should().NotBeNull();
        config.Capabilities.Should().SatisfyRespectively(
            capability => capability.Name.Should().Be("nested_virtualization"));
    }

    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_indentation_style_object_fodder_content_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(
            FodderContentIndentationStyleObjectCatletYaml);

        config.Should().NotBeNull();
        config.Fodder.Should().SatisfyRespectively(
            fodder => fodder.Content.Should().Be(FodderContentIndentationStyleObjectYaml.ReplaceLineEndings("\n")));
    }

    /*
    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_flow_style_object_fodder_content_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(
            FodderContentFlowStyleObjectCatletYaml);

        config.Should().NotBeNull();
        config.Fodder.Should().SatisfyRespectively(
            fodder => fodder.Content.Should().Be(FodderContentFlowStyleObjectYaml));
    }

    [CulturedFact("en-US", "de-DE")]
    public void Convert_from_indentation_style_sequence_fodder_content_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(
            FodderContentIndentationStyleSequenceCatletYaml);

        config.Should().NotBeNull();
        config.Fodder.Should().SatisfyRespectively(
            fodder => fodder.Content.Should().Be(FodderContentIndentationStyleSequenceYaml));
    }

    [CulturedFact("en-US", "de-DE")]
    [InlineData(FodderContentIndentationStyleObjectCatletYaml, FodderContentIndentationStyleObjectYaml)]
    public void Convert_from_flow_style_sequence_fodder_content_yaml()
    {
        var config = CatletConfigYamlSerializer.Deserialize(
            FodderContentFlowStyleSequenceCatletYaml);

        config.Should().NotBeNull();
        config.Fodder.Should().SatisfyRespectively(
            fodder => fodder.Content.Should().Be(FodderContentFlowStyleSequenceYaml));
    }
    */

    [CulturedFact("en-US", "de-DE")]
    public void Convert_foos()
    {
        var yaml = """
                   name: 42
                   name: 42
                   """;

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

        config.Should().NotBeNull();
    }

}

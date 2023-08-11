using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Yaml;
using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Catlet.Tests
{
    public class YamlConverterTests : ConverterTestBase
    {

      private const string SampleYaml1 = @"project: homeland
name: cinc-windows
label: cinc
hostname: cinc-host
environment: world
datastore: home
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
  label: cinc-shared
  datastore: ds2
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
";
      
      private const string SampleYaml2 = @"parent: dbosoft/winsrv2019-standard/20220324";

      private const string SampleYaml3 = @"
parent: dbosoft/winsrv2019-standard/20220324  
cpu: 4
";      
      
      private const string SampleYaml4 = @"
capabilities:
  - nested_virtualization
";           
      
        [Fact]
        public void Converts_from_yaml()
        {
          var serializer = new DeserializerBuilder()
            .Build();
          
            var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleYaml1);
            var config = CatletConfigDictionaryConverter.Convert(dictionary, true);
            AssertSample1(config);
        }
        
        [Theory()]
        [InlineData(SampleYaml1, SampleYaml1)]
        public void Converts_To_yaml(string input, string expected)
        {
          var config = CatletConfigYamlSerializer.Deserialize(input);
          var act = CatletConfigYamlSerializer.Serialize(config);
          act.Should().Be(expected);

        }

        [Fact]
        public void Convert_from_minimal_yaml()
        {
            var config = CatletConfigYamlSerializer.Deserialize(SampleYaml2);

            config.Should().NotBeNull();
            config.Parent.Should().Be("dbosoft/winsrv2019-standard/20220324");

        }

        [Fact]
        public void Convert_from_short_cpu_yaml()
        {
            var config = CatletConfigYamlSerializer.Deserialize(SampleYaml3);

            config.Should().NotBeNull();
            config.Cpu.Should().NotBeNull();
            config.Cpu?.Count.Should().Be(4);

        }
        
        [Fact]
        public void Convert_from_short_features_yaml()
        {
          var config = CatletConfigYamlSerializer.Deserialize(SampleYaml4);

          config.Should().NotBeNull();
          config.Capabilities.Should().NotBeNull();
          config.Capabilities.Should().HaveCount(1);
          config.Capabilities?[0].Name.Should().Be("nested_virtualization");

        }
    }
}
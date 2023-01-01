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

      private const string SampleYaml1 = @"name: cinc-windows
environment: env1
project: cinc
vcatlet:
  slug: cinc-slug
  data_store: ds1
  image: dbosoft/winsrv2019-standard/20220324
  cpu:
    count: 4
  memory:
    startup: 1024
    minimum: 512
    maximum: 4096
  drives:
  - name: data
    slug: cinc-shared
    data_store: ds2
    template: some_template
    size: 1
    type: SharedVHD
  network_adapters:
  - name: eth0
    mac_address: 4711
  - name: eth1
    mac_address: 4712
  features:
  - name: nested_virtualization
  - name: secure_boot
    settings:
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
raising:
  hostname: cinc-host
  config:
  - name: admin-windows
    type: cloud-config
    content: >-
      users:
        - name: Admin
          groups: [ ""Administrators"" ]
          passwd: InitialPassw0rd
    file_name: filename
    sensitive: true
";
      
      private const string SampleYaml2 = @"vCatlet: dbosoft/winsrv2019-standard/20220324";

      private const string SampleYaml3 = @"
vcatlet:
  image: dbosoft/winsrv2019-standard/20220324  
  cpu: 4
";      
      
      private const string SampleYaml4 = @"
vcatlet:
  features:
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

            config.VCatlet.Should().NotBeNull();
            config.VCatlet.Image.Should().Be("dbosoft/winsrv2019-standard/20220324");

        }

        [Fact]
        public void Convert_from_short_cpu_yaml()
        {
            var config = CatletConfigYamlSerializer.Deserialize(SampleYaml3);

            config.VCatlet.Should().NotBeNull();
            config.VCatlet.Cpu.Should().NotBeNull();
            config.VCatlet.Cpu.Count.Should().Be(4);

        }
        
        [Fact]
        public void Convert_from_short_features_yaml()
        {
          var config = CatletConfigYamlSerializer.Deserialize(SampleYaml4);

          config.VCatlet.Should().NotBeNull();
          config.VCatlet.Features.Should().NotBeNull();
          config.VCatlet.Features.Should().HaveCount(1);
          config.VCatlet.Features[0].Name.Should().Be("nested_virtualization");

        }
    }
}
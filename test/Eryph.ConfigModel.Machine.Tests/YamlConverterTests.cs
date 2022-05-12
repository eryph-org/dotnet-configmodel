using System.Collections.Generic;
using FluentAssertions;
using SharpYaml.Serialization;
using Xunit;

namespace Eryph.ConfigModel.Machine.Tests
{
    public class YamlConverterTests : ConverterTestBase
    {

        private const string SampleYaml1 = @"

name: cinc-windows
project: cinc
environment: env1
vm:
  slug: cinc-slug
  datastore: ds1
  image: dbosoft/winsrv2019-standard/20220324  
  cpu: 
    count: 4
  drives:
    - name: data
      size: 1
      slug: cinc-shared
      datastore: ds2
      template: some_template
      type: SharedVHD

  network_adapters:
    - name: eth1
      mac_address: 4711
  memory: 
    startup: 1024
    minimum: 512
    maximum: 4096
networks:
 - name: lan
   adapter_name: eth2

provisioning:  
 hostname: cinc-host
 config:  
 - name: admin-windows
   type: cloud-config
   sensitive: true
   filename: filename
   content: |
    users:
      - name: Admin
        groups: [ ""Administrators"" ]
        passwd: InitialPassw0rd";

        private const string SampleYaml2 = @"vm: dbosoft/winsrv2019-standard/20220324";

        private const string SampleYaml3 = @"
vm:
  image: dbosoft/winsrv2019-standard/20220324  
  cpu: 4
";
        
        [Fact]
        public void Converts_from_yaml()
        {
            var serializer = new Serializer();
            var dictionary = serializer.Deserialize<Dictionary<string, object>>(SampleYaml1);
            var config = MachineConfigDictionaryConverter.Convert(dictionary, true);
            AssertSample1(config);
        }

        [Fact]
        public void Convert_from_minimal_yaml()
        {
            var serializer = new Serializer();
            var dictionary = serializer.Deserialize<Dictionary<string, object>>(SampleYaml2);
            var config = MachineConfigDictionaryConverter.Convert(dictionary, true);

            config.VM.Should().NotBeNull();
            config.VM.Image.Should().Be("dbosoft/winsrv2019-standard/20220324");

        }

        [Fact]
        public void Convert_from_short_cpu_yaml()
        {
            var serializer = new Serializer();
            var dictionary = serializer.Deserialize<Dictionary<string, object>>(SampleYaml3);
            var config = MachineConfigDictionaryConverter.Convert(dictionary, true);

            config.VM.Should().NotBeNull();
            config.VM.Cpu.Should().NotBeNull();
            config.VM.Cpu.Count.Should().Be(4);

        }
    }
}
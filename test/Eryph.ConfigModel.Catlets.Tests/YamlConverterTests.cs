using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Catlet.Tests
{
    public class YamlConverterTests : ConverterTestBase
    {

        private const string SampleYaml1 = @"

name: cinc-windows
project: cinc
environment: env1
vcatlet:
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
    - name: eth0
      mac_address: 4711
    - name: eth1
      mac_address: 4712
  memory: 
    startup: 1024
    minimum: 512
    maximum: 4096
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
   sensitive: true
   filename: filename
   content: |
    users:
      - name: Admin
        groups: [ ""Administrators"" ]
        passwd: InitialPassw0rd";

        private const string SampleYaml2 = @"vCatlet: dbosoft/winsrv2019-standard/20220324";

        private const string SampleYaml3 = @"
vcatlet:
  image: dbosoft/winsrv2019-standard/20220324  
  cpu: 4
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

        [Fact]
        public void Convert_from_minimal_yaml()
        {
          var serializer = new DeserializerBuilder()
            .Build();
          
            var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleYaml2);
            var config = CatletConfigDictionaryConverter.Convert(dictionary, true);

            config.VCatlet.Should().NotBeNull();
            config.VCatlet.Image.Should().Be("dbosoft/winsrv2019-standard/20220324");

        }

        [Fact]
        public void Convert_from_short_cpu_yaml()
        {
          var serializer = new DeserializerBuilder()
            .Build();
            var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleYaml3);
            var config = CatletConfigDictionaryConverter.Convert(dictionary, true);

            config.VCatlet.Should().NotBeNull();
            config.VCatlet.Cpu.Should().NotBeNull();
            config.VCatlet.Cpu.Count.Should().Be(4);

        }
    }
}
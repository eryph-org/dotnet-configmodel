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


project: cinc
networks:
- name: default
  address: 192.168.2.0/24      
  provider:
    name: default
    subnet: provider_subnet
    ip_pool: other_pool

  subnets: 
  - name: subnet_name
    address: 192.168.2.0/23
    dns_servers:
    - 1.2.3.4
    - 5.6.7.8
    mtu: 1300
        
    ip_pools: 
    - name: pool_name
      first_ip: 192.168.2.10
      last_ip: 192.168.2.100

- name: default
  environment: dev
  provider: default
  address: 192.168.3.0/24  
    
";

        
        [Fact]
        public void Converts_from_yaml()
        {
          var serializer = new DeserializerBuilder()
              .Build();
          
            var dictionary = serializer.Deserialize<Dictionary<object, object>>(SampleYaml1);
            var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, true);
            AssertSample1(config);
        }

        
    }
}
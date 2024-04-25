namespace Eryph.ConfigModel.Catlet.Tests;

public static class Samples
{
  public const string Json1 = @"{
  ""version"": ""1.0"",
  ""project"": ""cinc"",
  ""networks"": [
    {
      ""name"": ""default"",
      ""address"": ""192.168.2.0/24"",
      ""provider"": {
        ""name"": ""default"",
        ""subnet"": ""provider_subnet"",
        ""ipPool"": ""other_pool""
      },
      ""subnets"": [
        {
          ""name"": ""subnet_name"",
          ""address"": ""192.168.2.0/23"",
          ""ipPools"": [
            {
              ""name"": ""pool_name"",
              ""firstIp"": ""192.168.2.10"",
              ""lastIp"": ""192.168.2.100"",
              ""nextIp"": ""192.168.2.20""
            }
          ],
          ""dnsServers"": [
            ""1.2.3.4"",
            ""5.6.7.8""
          ],
          ""dnsDomain"": ""jabba.beng"",
          ""mtu"": 1300
        }
      ]
    },
    {
      ""name"": ""default"",
      ""environment"": ""dev"",
      ""address"": ""192.168.3.0/24"",
      ""provider"": {
        ""name"": ""default""
      }
    }
  ]
}";

  public const string Yaml1 = @"version: 1.0
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
    ip_pools:
    - name: pool_name
      first_ip: 192.168.2.10
      last_ip: 192.168.2.100
      next_ip: 192.168.2.20
    dns_servers:
    - 1.2.3.4
    - 5.6.7.8
    dns_domain: jabba.beng
    mtu: 1300
- name: default
  environment: dev
  address: 192.168.3.0/24
  provider:
    name: default
";
}
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class JsonConverterTests: ConverterTestBase
{
        

    private const string SampleJson1 = @"

{
  ""project"": ""cinc"",
  ""networks"": [
    {
      ""name"": ""default"",
      ""address"": ""192.168.2.0/24"",
      ""provider"": {
        ""name"": ""default"",
        ""subnet"": ""provider_subnet"",
        ""ip_pool"": ""other_pool""
      },
      ""subnets"": [
        {
          ""name"": ""subnet_name"",
          ""address"": ""192.168.2.0/23"",
          ""dns_servers"": [
            ""1.2.3.4"",
            ""5.6.7.8""
          ],
          ""mtu"": 1300,
          ""ip_pools"": [
            {
              ""name"": ""pool_name"",
              ""first_ip"": ""192.168.2.10"",
              ""last_ip"": ""192.168.2.100""
            }
          ]
        }
      ]
    },
    {
      ""name"": ""default"",
      ""environment"": ""dev"",
      ""provider"": {
        ""name"": ""default""
      },
      ""address"": ""192.168.3.0/24""
    }
  ]
}

";
    [Fact]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = ProjectNetworksConfigDictionaryConverter.Convert(dictionary, false);
        AssertSample1(config);

    }


}
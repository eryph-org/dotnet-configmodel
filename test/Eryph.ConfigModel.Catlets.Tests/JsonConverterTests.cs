using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class JsonConverterTests: ConverterTestBase
{
        

    private const string SampleJson1 = @"
{
    ""name"": ""cinc-windows"",
    ""project"": ""cinc"",
    ""environment"": ""env1"",
    ""vCatlet"": 
    {
        ""slug"": ""cinc-slug"",
        ""datastore"": ""ds1"",
         ""image"": ""dbosoft/winsrv2019-standard/20220324"",
         ""cpu"": {
            ""count"": 4
        },
         ""memory"": {
            ""startup"": 1024,
            ""minimum"": 512,
            ""maximum"": 4096
        },
        ""drives"": [{
            ""name"": ""data"",
            ""size"": ""1"",
            ""slug"": ""cinc-shared"",
            ""template"": ""some_template"",
            ""datastore"": ""ds2"",
            ""type"": ""SharedVHD""
        }],
        ""network_adapters"": [{
            ""name"": ""eth0"",
            ""macAddress"": ""4711""
        },
        {
            ""name"": ""eth1"",
            ""macAddress"": ""4712""
        }]
    },
    ""networks"": [
        {
            ""name"": ""default"",
            ""adapterName"": ""eth0"",
            ""subnetV4"": {
                ""name"": ""other"",
                ""ipPool"": ""other_pool""
            },
            ""subnetV6"": {
                ""name"": ""other_v6""
            }
        },
        {
            ""name"": ""backup"",
            ""adapterName"": ""eth1""
        }   
    ],
    ""raising"": {    
        ""hostname"": ""cinc-host"",
        ""config"": [{
                ""name"": ""admin-windows"",
                ""filename"": ""filename"",
                ""type"": ""cloud-config"",
                ""sensitive"": true,
                ""content"":""users:\n  - name: Admin\ngroups: [ \""Administrators\"" ]\n  passwd: InitialPassw0rd""
       }
        ]
}
}
";
    [Fact]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = CatletConfigDictionaryConverter.Convert(dictionary);
        AssertSample1(config);
    }


}
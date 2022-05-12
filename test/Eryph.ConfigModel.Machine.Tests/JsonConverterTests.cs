using Eryph.ConfigModel.Json;
using Xunit;

namespace Eryph.ConfigModel.Machine.Tests;

public class JsonConverterTests: ConverterTestBase
{
        

    private const string SampleJson1 = @"
{
    ""name"": ""cinc-windows"",
    ""project"": ""cinc"",
    ""environment"": ""env1"",
    ""vm"": 
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
            ""name"": ""eth1"",
            ""mac_address"": ""4711""
        }]
    },
    ""networks"": [
        {
            ""name"": ""lan"",
            ""adapterName"": ""eth2""
        }   
    ],
    ""provisioning"": {    
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
        var dictionary = JsonToDictionary.Deserialize(SampleJson1);
        var config = MachineConfigDictionaryConverter.Convert(dictionary);
        AssertSample1(config);
    }


}
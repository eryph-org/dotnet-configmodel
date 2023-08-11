using System.Text.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class JsonConverterTests: ConverterTestBase
{
    
    private const string SampleJson1 = $@"{{
  ""society"": ""homeland"",
  ""name"": ""cinc-windows"",
  ""label"": ""cinc"",
  ""socialName"": ""cinc-host"",
  ""environment"": ""world"",
  ""lair"": ""home"",
  ""parent"": ""dbosoft/winsrv2019-standard/20220324"",
  ""cpu"": {{
    ""count"": 4
  }},
  ""memory"": {{
    ""startup"": 1024,
    ""minimum"": 512,
    ""maximum"": 4096
  }},
  ""drives"": [
    {{
      ""name"": ""data"",
      ""mutation"": ""Overwrite"",
      ""label"": ""cinc-shared"",
      ""lair"": ""ds2"",
      ""parent"": ""some_template"",
      ""size"": 1,
      ""type"": ""SharedVHD""
    }}
  ],
  ""networkAdapters"": [
    {{
      ""name"": ""eth0"",
      ""macAddress"": ""4711""
    }},
    {{
      ""name"": ""eth1"",
      ""mutation"": ""Remove"",
      ""macAddress"": ""4712""
    }}
  ],
  ""capabilities"": [
    {{
      ""name"": ""nested_virtualization""
    }},
    {{
      ""name"": ""secure_boot"",
      ""mutation"": ""Remove"",
      ""details"": [
        ""tpm"",
        ""shielded""
      ]
    }}
  ],
  ""networks"": [
    {{
      ""name"": ""default"",
      ""adapterName"": ""eth0"",
      ""subnetV4"": {{
        ""name"": ""other"",
        ""ipPool"": ""other_pool""
      }},
      ""subnetV6"": {{
        ""name"": ""other_v6""
      }}
    }},
    {{
      ""name"": ""backup"",
      ""adapterName"": ""eth1""
    }}
  ],
  ""fodder"": [
    {{
      ""name"": ""admin-windows"",
      ""type"": ""cloud-config"",
      ""content"": ""users:\n  - name: Admin\ngroups: [ \u0022Administrators\u0022 ]\n  passwd: InitialPassw0rd"",
      ""fileName"": ""filename"",
      ""secret"": true
    }}
  ]
}}";
    [Fact]
    public void Converts_from_json()
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(SampleJson1);
        var config = CatletConfigDictionaryConverter.Convert(dictionary);
        AssertSample1(config);
    }

    [Theory]
    [InlineData(SampleJson1, SampleJson1)]
    public void Converts_to_json(string input, string expected)
    {
        var dictionary = ConfigModelJsonSerializer.DeserializeToDictionary(input);
        var config = CatletConfigDictionaryConverter.Convert(dictionary, false);

        var copyOptions = new JsonSerializerOptions(ConfigModelJsonSerializer.DefaultOptions)
        {
            WriteIndented = true
        };
        var act = ConfigModelJsonSerializer.Serialize(config,copyOptions );
        act.Should().Be(expected);

    }
}
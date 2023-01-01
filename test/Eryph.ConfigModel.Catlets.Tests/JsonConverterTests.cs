using System.Text.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class JsonConverterTests: ConverterTestBase
{
    
    private const string SampleJson1 = $@"{{
  ""name"": ""cinc-windows"",
  ""environment"": ""env1"",
  ""project"": ""cinc"",
  ""vCatlet"": {{
    ""slug"": ""cinc-slug"",
    ""dataStore"": ""ds1"",
    ""image"": ""dbosoft/winsrv2019-standard/20220324"",
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
        ""slug"": ""cinc-shared"",
        ""dataStore"": ""ds2"",
        ""template"": ""some_template"",
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
        ""macAddress"": ""4712""
      }}
    ],
    ""features"": [
      {{
        ""name"": ""nested_virtualization""
      }},
      {{
        ""name"": ""secure_boot"",
        ""settings"": [
          ""tpm"",
          ""shielded""
        ]
      }}
    ]
  }},
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
  ""raising"": {{
    ""hostname"": ""cinc-host"",
    ""config"": [
      {{
        ""name"": ""admin-windows"",
        ""type"": ""cloud-config"",
        ""content"": ""users:\n  - name: Admin\ngroups: [ \u0022Administrators\u0022 ]\n  passwd: InitialPassw0rd"",
        ""fileName"": ""filename"",
        ""sensitive"": true
      }}
    ]
  }}
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ConfigModel.Yaml;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class CatletConfigYamlSerializerTests : SerializerTestBase
{
    [Fact]
    public void Deserialize_YamlWithScalarValues_ReturnsConfig()
    {
        var yaml =
            """
            cpu: 2
            memory: 1024
            
            """;

        var config = CatletConfigYamlSerializer.Deserialize(yaml);

        ShouldBeSimpleConfig(config);
    }

}

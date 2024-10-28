using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Variables;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public class ConverterTestBase
{
    protected static FodderGeneConfig ComplexConfig => new()
    {
        Name = "test-fodder",
        Version = "1.0",
        Fodder =
        [
            new FodderConfig()
            {
                Name = "first-food",
                Type = "cloud-config",
                Content = "abc\ndef",
                FileName = "first-food.txt",
                Remove = true,
                Secret = true,
            },
            new FodderConfig(),
        ],
        Variables =
        [
            new VariableConfig()
            {
                Name = "first-variable",
                Required = true,
                Secret = true,
                Type = VariableType.String,
                Value = "abc",
            },
            new VariableConfig(),
        ],
    };

    protected static void AssertSample1(FodderGeneConfig config)
    {
        config.Should().NotBeNull();
        config.Name.Should().Be("fodder1");
        config.Fodder.Should().NotBeNull();
        config.Fodder.Should().HaveCount(2);
        config.Fodder?[0].Name.Should().Be("admin-windows");
        config.Fodder?[0].Type.Should().Be("cloud-config");
        config.Fodder?[0].FileName.Should().Be("filename");
        config.Fodder?[0].Secret.Should().Be(true);
        config.Fodder?[0].Content.Should().Contain("- name: Admin");
        config.Fodder?[0].Content.Should().NotEndWith("\0");

        config.Fodder?[1].Name.Should().Be("super-dupa");
        config.Fodder?[1].Type.Should().Be("cloud-config");

        config.Variables.Should().SatisfyRespectively(
            variable =>
            {
                variable.Name.Should().Be("first");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("first value");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            },
            variable =>
            {
                variable.Name.Should().Be("second");
                variable.Type.Should().Be(VariableType.Boolean);
                variable.Value.Should().Be("true");
                variable.Required.Should().BeTrue();
                variable.Secret.Should().BeTrue();
            });
    }

    protected static void AssertNativeVariableValuesSample(FodderGeneConfig config)
    {
        config.Variables.Should().SatisfyRespectively(
            variable =>
            {
                variable.Name.Should().Be("boolean");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("true");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            },
            variable =>
            {
                variable.Name.Should().Be("number");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("-4.2");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            });
    }
}

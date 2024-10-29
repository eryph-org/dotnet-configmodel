using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Variables;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public abstract class FodderGeneConfigSerializerTestBase
{
    protected static void AssertComplexConfig(FodderGeneConfig config)
    {
        config.Should().NotBeNull();
        config.Name.Should().Be("fodder1");

        config.Fodder.Should().SatisfyRespectively(
            fodder =>
            {
                fodder.Name.Should().Be("admin-windows");
                fodder.Type.Should().Be("cloud-config");
                fodder.FileName.Should().Be("filename");
                fodder.Remove.Should().Be(true);
                fodder.Secret.Should().Be(true);
                fodder.Content.Should().Contain("- name: Admin");
                fodder.Content.Should().NotEndWith("\0");
            },
            fodder =>
            {
                fodder.Name.Should().Be("super-dupa");
                fodder.Type.Should().Be("cloud-config");
                fodder.FileName.Should().BeNull();
                fodder.Remove.Should().BeNull();
                fodder.Secret.Should().BeNull();
                fodder.Content.Should().BeNull();
                fodder.Variables.Should().BeNull();
            });

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
}

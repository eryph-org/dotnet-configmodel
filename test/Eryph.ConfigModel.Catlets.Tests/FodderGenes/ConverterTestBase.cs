using Eryph.ConfigModel.FodderGenes;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests.FodderGenes;

public class ConverterTestBase
{
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
    }
}
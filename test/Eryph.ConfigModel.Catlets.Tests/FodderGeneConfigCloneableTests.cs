using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Variables;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests;

public class FodderGeneConfigCloneableTests
{
    [Fact]
    public void FodderGeneConfig_is_cloned()
    {
        var config = new FodderGeneConfig
        {
            Name = "fodder1",
            Requires = ["acme/acme-tools"],
            Variables = [new VariableConfig { Name = "geneVariable" }],
            Fodder = [new FodderConfig { Name = "food" }],
        };

        var clonedConfig = config.Clone();

        clonedConfig.Requires.Should().Equal(config.Requires);
        clonedConfig.Requires.Should().NotBeSameAs(config.Requires);

        clonedConfig.Variables.Should().NotBeNull();
        clonedConfig.Variables.Should().NotBeSameAs(config.Variables);

        clonedConfig.Fodder.Should().NotBeNull();
        clonedConfig.Fodder.Should().NotBeSameAs(config.Fodder);
    }

    [Fact]
    public void FodderGeneConfig_without_requires_is_cloned()
    {
        var config = new FodderGeneConfig { Name = "fodder1" };

        var clonedConfig = config.Clone();

        clonedConfig.Requires.Should().BeNull();
    }
}

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class GeneRequirementTests
{
    [Theory]
    [InlineData("acme/acme-tools")]
    [InlineData("ACME/ACME-Tools")]
    public void New_GeneSetOnly_ReturnsValue(string value)
    {
        var result = GeneRequirement.New(value);

        result.Value.Should().Be("acme/acme-tools");
        result.Organization.Value.Should().Be("acme");
        result.GeneSet.Value.Should().Be("acme-tools");
        result.GeneName.Should().BeNone();
    }

    [Theory]
    [InlineData("acme/acme-tools:pwsh")]
    [InlineData("ACME/ACME-Tools:PWSH")]
    public void New_WithGeneName_ReturnsValue(string value)
    {
        var result = GeneRequirement.New(value);

        result.Value.Should().Be("acme/acme-tools:pwsh");
        result.Organization.Value.Should().Be("acme");
        result.GeneSet.Value.Should().Be("acme-tools");
        result.GeneName.Should().BeSome().Which.Value.Should().Be("pwsh");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("acme/acme-tools:")]
    [InlineData("acme//acme-tools")]
    [InlineData("/")]
    [InlineData(":pwsh")]
    [InlineData(" acme/acme-tools")]
    [InlineData("acme/acme-tools:a-very-long-gene-name")]
    [InlineData("GENE:acme/acme-tools")]
    public void NewValidation_MalformedValue_ReturnsFail(string value)
    {
        var result = GeneRequirement.NewValidation(value);

        result.Should().BeFail();
    }

    [Theory]
    [InlineData("acme/acme-tools/latest", "The gene requirement must not contain a tag. The tag is chosen by the catlet.")]
    [InlineData("acme/acme-tools/1.0:pwsh", "The gene requirement must not contain a tag. The tag is chosen by the catlet.")]
    [InlineData("gene:acme/acme-tools:pwsh", "The gene requirement must not start with 'gene:'. It must be org/geneset or org/geneset:genename.")]
    [InlineData("acme", "The gene requirement is malformed. It must be org/geneset or org/geneset:genename.")]
    [InlineData("acme/acme-tools:pwsh:extra", "The gene requirement is malformed. It must be org/geneset or org/geneset:genename.")]
    public void NewValidation_InvalidValue_ReturnsFail(string value, string expectedMessage)
    {
        var result = GeneRequirement.NewValidation(value);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should().Be(expectedMessage));
    }

    [Theory]
    [InlineData("acme/acme-tools", "gene:acme/acme-tools/1.0:pwsh", true)]
    [InlineData("acme/acme-tools", "gene:acme/acme-tools/latest:winget", true)]
    [InlineData("acme/acme-tools:pwsh", "gene:acme/acme-tools/1.0:pwsh", true)]
    [InlineData("ACME/ACME-Tools:PWSH", "gene:acme/acme-tools/1.0:pwsh", true)]
    [InlineData("acme/acme-tools:pwsh", "gene:acme/acme-tools/1.0:winget", false)]
    [InlineData("acme/acme-tools", "gene:acme/acme-other/1.0:pwsh", false)]
    [InlineData("acme/acme-tools", "gene:other/acme-tools/1.0:pwsh", false)]
    public void IsSatisfiedBy_ReturnsExpectedResult(string requirement, string geneId, bool expected)
    {
        var result = GeneRequirement.New(requirement).IsSatisfiedBy(GeneIdentifier.New(geneId));

        result.Should().Be(expected);
    }
}

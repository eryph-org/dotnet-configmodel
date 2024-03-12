using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class GeneSetIdentifierTests
{
    [Theory]
    [InlineData("acme", "acme-linux", "1.0-rc.1")]
    [InlineData("ACME", "ACME-Linux", "1.0-RC.1")]
    public void New_ComplexDataWithTag_ReturnsValue(string organisation, string geneset, string tag)
    {
        var result = new GeneSetIdentifier(
            OrganizationName.New(organisation),
            GeneSetName.New(geneset),
            TagName.New(tag));

        Validate(result, "1.0-rc.1");
    }

    [Theory]
    [InlineData("acme", "acme-linux")]
    [InlineData("ACME", "ACME-Linux")]
    public void New_ComplexDataWithoutTag_ReturnsValue(string organisation, string geneset)
    {
        var result = new GeneSetIdentifier(
            OrganizationName.New(organisation),
            GeneSetName.New(geneset));
        
        Validate(result, "latest");
    }

    [Theory]
    [InlineData("acme/acme-linux/1.0-rc.1")]
    [InlineData("ACME/ACME-Linux/1.0-RC.1")]
    public void New_StringWithTag_ReturnsValue(string id)
    {
        var result = GeneSetIdentifier.New(id);

        Validate(result, "1.0-rc.1");
    }

    [Theory]
    [InlineData("acme/acme-linux")]
    [InlineData("ACME/ACME-Linux")]
    public void New_StringWithoutTag_ReturnsValue(string id)
    {
        var result = GeneSetIdentifier.New(id);

        Validate(result, "latest");
    }

    [Theory]
    [InlineData("acme/acme-linux/latest")]
    [InlineData("ACME/ACME-Linux/Latest")]
    public void New_StringWithLatestTag_ReturnsValue(string id)
    {
        var result = GeneSetIdentifier.New(id);

        Validate(result, "latest");
    }

    private void Validate(GeneSetIdentifier geneSetIdentifier, string expectedTag)
    {
        geneSetIdentifier.Value.Should().Be($"acme/acme-linux/{expectedTag}");
        geneSetIdentifier.ValueWithoutTag.Should().Be("acme/acme-linux");
        geneSetIdentifier.Organization.Value.Should().Be("acme");
        geneSetIdentifier.GeneSet.Value.Should().Be("acme-linux");
        geneSetIdentifier.Tag.Value.Should().Be(expectedTag);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class GeneSetIdentifierTests
{
    [Fact]
    public void New_ComplexDataWithTag_ReturnsValue()
    {
        var geneSetIdentifier = new GeneSetIdentifier(
            OrganizationName.New("acme"),
            GeneSetName.New("acme-linux"),
            TagName.New("1.0"));

        geneSetIdentifier.Value.Should().Be("acme/acme-linux/1.0");
    }

    [Fact]
    public void New_ComplexDataWithoutTag_ReturnsValue()
    {
        var geneSetIdentifier = new GeneSetIdentifier(
            OrganizationName.New("acme"),
            GeneSetName.New("acme-linux"));

        geneSetIdentifier.Value.Should().Be("acme/acme-linux");
    }

    [Fact]
    public void New_StringWithTag_ReturnsValue()
    {
        string id = "acme/acme-linux/1.0";
        
        var geneSetIdentifier = GeneSetIdentifier.New(id);

        geneSetIdentifier.Value.Should().Be(id);
        geneSetIdentifier.Organization.Value.Should().Be("acme");
        geneSetIdentifier.GeneSet.Value.Should().Be("acme-linux");
        geneSetIdentifier.Tag.Value.Should().Be("1.0");
    }

    [Fact]
    public void New_StringWithoutTag_ReturnsValue()
    {
        string id = "acme/acme-linux";

        var geneSetIdentifier = GeneSetIdentifier.New(id);

        geneSetIdentifier.Value.Should().Be(id);
        geneSetIdentifier.Organization.Value.Should().Be("acme");
        geneSetIdentifier.GeneSet.Value.Should().Be("acme-linux");
        geneSetIdentifier.Tag.Value.Should().Be("latest");
    }

    [Fact]
    public void New_StringWithLatestTag_ReturnsValue()
    {
        string id = "acme/acme-linux/latest";

        var geneSetIdentifier = GeneSetIdentifier.New(id);

        geneSetIdentifier.Value.Should().Be(id);
        geneSetIdentifier.Organization.Value.Should().Be("acme");
        geneSetIdentifier.GeneSet.Value.Should().Be("acme-linux");
        geneSetIdentifier.Tag.Value.Should().Be("latest");
    }
}

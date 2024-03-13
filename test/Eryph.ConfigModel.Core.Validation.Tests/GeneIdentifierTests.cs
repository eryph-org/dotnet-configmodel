using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class GeneIdentifierTests
{
    [Theory]
    [InlineData("acme/acme-linux", "catlet")]
    [InlineData("ACME/ACME-Linux", "Catlet")]
    [InlineData("acme/acme-linux/latest", "catlet")]
    [InlineData("ACME/ACME-Linux/Latest", "Catlet")]
    public void New_ComplexData_ReturnsValue(string geneSetId, string geneName)
    {
        var result = new GeneIdentifier(
            GeneSetIdentifier.New("acme/acme-linux/latest"),
            GeneName.New("catlet"));

        Validate(result);
    }

    [Theory]
    [InlineData("gene:acme/acme-linux:catlet")]
    [InlineData("gene:ACME/ACME-Linux:Catlet")]
    [InlineData("gene:acme/acme-linux/latest:catlet")]
    [InlineData("gene:ACME/ACME-Linux/Latest:Catlet")]
    public void New_String_ReturnsValue(string geneId)
    {
        var result = GeneIdentifier.New(geneId);

        Validate(result);
    }

    private void Validate(GeneIdentifier geneId)
    {
        geneId.Value.Should().Be("gene:acme/acme-linux/latest:catlet");
        geneId.GeneSet.Value.Should().Be("acme/acme-linux/latest");
        geneId.GeneName.Value.Should().Be("catlet");
    }
}

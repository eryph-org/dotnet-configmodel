using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class GeneIdentifierTests
{
    [Fact]
    public void New_ComplexData_ReturnsValue()
    {
        var geneIdentifier = new GeneIdentifier(
            GeneSetIdentifier.New("acme/acme-linux/latest"),
            GeneName.New("catlet"));

        geneIdentifier.Value.Should().Be("gene:acme/acme-linux/latest:catlet");
    }

    [Fact]
    public void New_String_ReturnsValue()
    {
        string id = "gene:acme/acme-linux/latest:catlet";

        var geneIdentifier = GeneIdentifier.New(id);

        geneIdentifier.Value.Should().Be(id);
        geneIdentifier.GeneSet.Value.Should().Be("acme/acme-linux/latest");
        geneIdentifier.GeneName.Value.Should().Be("catlet");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class FodderKeyTests
{
    [Fact]
    public void Create_NoSource_ReturnsKeyWithoutSource()
    {
        var result = FodderKey.Create("test-food", null);

        var fodderKey = result.Should().BeRight().Subject;
        fodderKey.Name.Should().Be(FodderName.New("test-food"));
        fodderKey.Source.Should().BeNone();
    }

    [Fact]
    public void Create_CatletGeneSource_ReturnsKeyWithoutSource()
    {
        var result = FodderKey.Create("test-food", "gene:acme/acme-os:catlet");

        var fodderKey = result.Should().BeRight().Subject;
        fodderKey.Name.Should().Be(FodderName.New("test-food"));
        fodderKey.Source.Should().BeNone();
    }

    [Fact]
    public void Create_FodderGeneSource_ReturnsKeyWithSource()
    {
        var result = FodderKey.Create("test-food", "gene:acme/acme-tools:test-fodder");

        var fodderKey = result.Should().BeRight().Subject;
        fodderKey.Name.Should().Be(FodderName.New("test-food"));
        fodderKey.Source.Should().Be(GeneIdentifier.New("gene:acme/acme-tools:test-fodder"));
    }

    [Fact]
    public void Create_InvalidName_ReturnsError()
    {
        var result = FodderKey.Create("invalid|name", null);

        result.Should().BeLeft().Which.Message
            .Should().Be("Found fodder with invalid name.");
    }

    [Fact]
    public void Create_InvalidSource_ReturnsError()
    {
        var result = FodderKey.Create(null, "invalid|source");

        result.Should().BeLeft().Which.Message
            .Should().Be("Found fodder with invalid source.");
    }
    
    [Fact]
    public void Create_WithoutNameAndSource_ReturnsError()
    {
        var result = FodderKey.Create(null, null);

        result.Should().BeLeft().Which.Message
            .Should().Be("Found invalid fodder with neither name nor source.");
    }

    [Fact]
    public void Create_CatletGeneWithoutName_ReturnsError()
    {
        var result = FodderKey.Create(null, "gene:acme/acme-os:catlet");

        result.Should().BeLeft().Which.Message
            .Should().Be("Found catlet fodder 'gene:acme/acme-os/latest:catlet' without valid name.");
    }

    [Fact]
    public void ToString_WithoutSource_ReturnsCorrectString()
    {
        var result = FodderKey.Create("test-food", null);

        result.Should().BeRight().Which.ToString()
            .Should().Be("catlet->test-food");
    }

    [Fact]
    public void ToString_WithSourceAndWithoutName_ReturnsCorrectString()
    {
        var result = FodderKey.Create(null, "gene:acme/acme-tools:test-fodder");

        result.Should().BeRight().Which.ToString()
            .Should().Be("gene:acme/acme-tools/latest:test-fodder");
    }

    [Fact]
    public void ToString_WithSourceAndName_ReturnsCorrectString()
    {
        var result = FodderKey.Create("test-food", "gene:acme/acme-tools:test-fodder");

        result.Should().BeRight().Which.ToString()
            .Should().Be("gene:acme/acme-tools/latest:test-fodder->test-food");
    }
}
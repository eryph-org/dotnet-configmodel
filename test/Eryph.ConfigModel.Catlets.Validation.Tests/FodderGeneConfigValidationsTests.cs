using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Variables;
using static Eryph.ConfigModel.FodderGeneConfigValidations;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class FodderGeneConfigValidationsTests
{
    [Theory]
    [InlineData("catlet")]
    public void ValidateFodderGeneConfig_FodderGeneNameIsReserved_ReturnsFail(
        string name)
    {
        var config = new FodderGeneConfig
        {
            Name = name,
            Fodder = new[]
            {
                new FodderConfig
                {
                    Name = "test-fodder",
                    Content = "test content",
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Name");
                issue.Message.Should().Be("The gene name 'catlet' is reserved.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_FodderWithSource_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig
                {
                    Name = "test-fodder",
                    Source = "gene:acme/acme-linux/latest:fodder",
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Source");
                issue.Message.Should().Be("References are not supported in fodder genes. The source must be empty.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_DataIsMissing_ReturnsFail()
    {
        var config = new FodderGeneConfig();

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Name");
                issue.Message.Should().Be("The Name is required.");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The list must have 1 or more entries.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_AddedFodderWithoutContent_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig
                {
                    Name = "test-fodder",
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0]");
                issue.Message.Should().Be("The content must be specified when adding fodder.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_FodderWithoutName_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Content = "test-content",
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Name");
                issue.Message.Should().Be("The Name is required.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_DuplicateFodderName_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "test-fodder",
                    Content = "first test content",
                },
                new FodderConfig()
                {
                    Name = "test-fodder",
                    Content = "second test content",
                },
            },

        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The fodder name 'test-fodder' is not unique.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_FodderWithVariables_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "test-fodder",
                    Content = "test-content",
                    Variables = new[]
                    {
                        new VariableConfig()
                        {
                            Name = "testVariable",
                        }
                    }
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Variables");
                issue.Message.Should().Be("Variables are not supported here.");
            });
    }
}

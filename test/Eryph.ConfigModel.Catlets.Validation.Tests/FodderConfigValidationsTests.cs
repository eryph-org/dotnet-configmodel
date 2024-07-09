using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ConfigModel.Variables;
using static Eryph.ConfigModel.FodderConfigValidations;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class FodderConfigValidationsTests
{
    [Fact]
    public void ValidateFodderConfig_ValidConfig_ReturnsSuccess()
    {
        var fodderConfig = new FodderConfig()
        {
            Name = "test-fodder",
            Content = "test content",
            FileName = "test-file-sh",
            Secret = true,
        };

        var result = ValidateFodderConfig(fodderConfig);

        result.Should().BeSuccess();
    }

    [Fact]
    public void ValidateFodderConfig_WithSourceAndAdditionalData_ReturnsFail()
    {
        var fodderConfig = new FodderConfig()
        {
            Name = "test-fodder",
            Source = "gene:acme/acme-fodder/1.0:my-fodder",
            Type = "shellscript",
            Content = "test content",
            FileName = "test-file-sh",
            Secret = true
        };

        var result = ValidateFodderConfig(fodderConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Type");
                issue.Message.Should().Be("The fodder type must not be specified when the fodder is a reference.");
            },
            issue =>
            {
                issue.Member.Should().Be("Content");
                issue.Message.Should().Be("The content must not be specified when the fodder is a reference.");
            },
            issue =>
            {
                issue.Member.Should().Be("FileName");
                issue.Message.Should().Be("The file name must not be specified when the fodder is a reference.");
            },
            issue =>
            {
                issue.Member.Should().Be("Secret");
                issue.Message.Should().Be("The secret flag must not be specified when the fodder is a reference.");
            });
    }

    [Fact]
    public void ValidateFodderConfig_WithRemoveAndAdditionalData_ReturnsFail()
    {
        var fodderConfig = new FodderConfig()
        {
            Name = "test-fodder",
            Remove = true,
            Type = "shellscript",
            Content = "test content",
            FileName = "test-file-sh",
            Secret = true,
            Variables =
            [
                new VariableConfig()
                {
                    Name  = "testVariable",
                }
            ]
        };

        var result = ValidateFodderConfig(fodderConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Type");
                issue.Message.Should().Be("The fodder type must not be specified when the fodder is removed.");
            },
            issue =>
            {
                issue.Member.Should().Be("Content");
                issue.Message.Should().Be("The content must not be specified when the fodder is removed.");
            },
            issue =>
            {
                issue.Member.Should().Be("FileName");
                issue.Message.Should().Be("The file name must not be specified when the fodder is removed.");
            },
            issue =>
            {
                issue.Member.Should().Be("Secret");
                issue.Message.Should().Be("The secret flag must not be specified when the fodder is removed.");
            },
            issue =>
            {
                issue.Member.Should().Be("Variables");
                issue.Message.Should().Be("The variables must not be specified when the fodder is removed.");
            });
    }
}

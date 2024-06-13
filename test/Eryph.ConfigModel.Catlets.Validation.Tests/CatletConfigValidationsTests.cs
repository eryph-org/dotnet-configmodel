using Eryph.ConfigModel.Variables;
using FluentAssertions;
using FluentAssertions.LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class CatletConfigValidationsTests
{
    [Fact]
    public void ValidateCatletConfig_EmptyConfig_ReturnsSuccess()
    {
        var catletConfig = new CatletConfig();
        
        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);
            
        result.Should().BeSuccess();
    }

    [Fact]
    public void ValidateCatletConfig_ConfigWithValidValues_ReturnsSuccess()
    {
        var catletConfig = new CatletConfig()
        {
            Project = "my-project",
            Parent = "acme/acme-os/1.0.0",
            Environment = "my-environment",
            Variables = new[]
            {
                new VariableConfig()
                {
                    Name = "myVariable",
                    Value = "my value",
                },
            },
            Cpu = new CatletCpuConfig()
            {
                Count = 1,
            },
            Drives = new[]
            {
                new CatletDriveConfig()
                {
                    Name = "sda"
                },
                new CatletDriveConfig()
                {
                    Name = "sdb"
                },
            },
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "my-fodder",
                    Source = "gene:acme/acme-fodder/1.0:my-fodder"
                },
            },
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeSuccess();
    }

    [Fact]
    public void ValidateCatletConfig_ConfigWithInvalidValues_ReturnsIssues()
    {
        var catletConfig = new CatletConfig()
        {
            Project = "my project",
            Environment = "my environment",
            Variables = new[]
            {
                new VariableConfig()
                {
                    Name = "my variable",
                    Type = VariableType.Boolean,
                    Value = "invalid value",
                },
            },
            Cpu = new CatletCpuConfig()
            {
                Count = -1,
            },
            Drives = new[]
            {
                new CatletDriveConfig()
                {
                    Name = "abc.bin"
                },
                new CatletDriveConfig()
                {
                    Name = "def.bin"
                },
            },
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "my fodder",
                    Source = "invalid source"
                },
            },
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Project");
                issue.Message.Should()
                    .Be("The project name contains invalid characters. Only latin characters, numbers, dots and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Environment");
                issue.Message.Should()
                    .Be("The environment name contains invalid characters. Only latin characters, numbers, dots and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Drives[0].Name");
                issue.Message.Should()
                    .Be("The catlet drive name contains invalid characters. Only latin characters, numbers and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Drives[1].Name");
                issue.Message.Should()
                    .Be("The catlet drive name contains invalid characters. Only latin characters, numbers and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Cpu.Count");
                issue.Message.Should()
                    .Be("The number of CPUs must be positive.");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Name");
                issue.Message.Should()
                    .Be("The fodder name contains invalid characters. Only latin characters, numbers, dots and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Source");
                issue.Message.Should()
                    .Be("The gene identifier is malformed. It must be gene:geneset:genename.");
            },
            issue =>
            {
                issue.Member.Should().Be("Variables[0].Name");
                issue.Message.Should()
                    .Be("The variable name contains invalid characters. Only latin characters and numbers are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Variables[0].Value");
                issue.Message.Should()
                    .Be("The value is not a valid boolean. Only 'true' and 'false' are allowed.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_AddedFodderWithoutContentOrSource_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "test-fodder",
                },
            },
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0]");
                issue.Message.Should().Be("The content or source must be specified when adding fodder.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_FodderWithoutNameOrSource_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Content = "test-content",
                },
            },
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0]");
                issue.Message.Should().Be("The name or source must be specified.");
            });
    }
}

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
                }
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
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Project");
                issue.Message.Should()
                    .Be("The project name contains invalid characters. Only lower case latin characters, numbers, dots and hyphens are permitted.");
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
            });
    }
}
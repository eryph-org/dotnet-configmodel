using FluentAssertions;
using FluentAssertions.LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;

namespace Eryph.ConfigModel.Catlets.Validation.Tests
{
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
                        Name = "abc.exe"
                    },
                    new CatletDriveConfig()
                    {
                        Name = "def.exe"
                    },
                },
            };

            var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

            var foo = result.Should().BeFail().Subject.ToList();

            var sub = result.Should().BeFail().Which.Should().SatisfyRespectively(
                issue =>
                {
                    issue.Member.Should().Be("Project");
                    issue.Message.Should()
                        .Be("The name must be between 1 and 50 lowercase letters or numbers and cannot start with p_");
                },
                issue =>
                {
                    issue.Member.Should().Be("Environment");
                    issue.Message.Should()
                        .Be("The name must be between 1 and 50 alphanumeric characters. Dashes and dot are allowed but not at the beginning or end.");
                },
                issue =>
                {
                    issue.Member.Should().Be("Drives[0].Name");
                    issue.Message.Should()
                        .Be("The name must be between 1 and 50 lowercase letters");
                },
                issue =>
                {
                    issue.Member.Should().Be("Drives[1].Name");
                    issue.Message.Should()
                        .Be("The name must be between 1 and 50 lowercase letters");
                },
                issue =>
                {
                    issue.Member.Should().Be("Cpu.Count");
                    issue.Message.Should()
                        .Be("The CPU count must be positive");
                });
        }
    }
}
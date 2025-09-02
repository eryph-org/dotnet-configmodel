using Eryph.ConfigModel.Variables;

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
            Parent = "acme/acme-os/1.0.0",
            Variables =
            [
                new VariableConfig()
                {
                    Name = "myVariable",
                    Value = "my value",
                }
            ],
            Capabilities = 
            [
                new CatletCapabilityConfig()
                {
                    Name = "secure_boot",
                    Details = ["template:MicrosoftUEFICertificateAuthority"]
                },
            ],
            Cpu = new CatletCpuConfig()
            {
                Count = 1,
            },
            Drives =
            [
                new CatletDriveConfig()
                {
                    Name = "sda"
                },
                new CatletDriveConfig()
                {
                    Name = "sdb"
                }
            ],
            Networks = 
            [
                new CatletNetworkConfig()
                {
                    Name = "default",
                    AdapterName = "eth0",
                },
            ],
            NetworkAdapters =
            [
                new CatletNetworkAdapterConfig()
                {
                    Name = "eth0",
                },
            ],
            Fodder =
            [
                new FodderConfig()
                {
                    Name = "my-fodder",
                    Content = "my fodder content",
                },
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder/1.0:my-fodder",
                },
                new FodderConfig()
                {
                    Name = "my-gene-fodder",
                    Source = "gene:acme/acme-fodder/1.0:my-fodder",
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeSuccess();
    }

    [Fact]
    public void ValidateCatletConfig_ConfigWithInvalidValues_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Project = "my project",
            Environment = "my environment",
            Variables =
            [
                new VariableConfig()
                {
                    Name = "my variable",
                    Type = VariableType.Boolean,
                    Value = "invalid value",
                }
            ],
            Capabilities =
            [
                new CatletCapabilityConfig()
                {
                    Name = "invalid|capability",
                    Details = []
                },
            ],
            Cpu = new CatletCpuConfig()
            {
                Count = -1,
            },
            Memory = new CatletMemoryConfig()
            {
                Startup = 129,
            },
            Drives =
            [
                new CatletDriveConfig()
                {
                    Name = "abc.bin"
                },
                new CatletDriveConfig()
                {
                    Name = "def.bin"
                }
            ],
            Networks =
            [
                new CatletNetworkConfig()
                {
                    Name = "invalid|network",
                    AdapterName = "invalid|adapter",
                },
            ],
            NetworkAdapters =
            [
                new CatletNetworkAdapterConfig()
                {
                    Name = "invalid|adapter",
                },
            ],
            Fodder =
            [
                new FodderConfig()
                {
                    Name = "my fodder",
                    Source = "invalid source"
                }
            ],
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
                issue.Member.Should().Be("Memory.Startup");
                issue.Message.Should()
                    .Be("The memory size must be a multiple of 128 MiB.");
            },
            issue =>
            {
                issue.Member.Should().Be("Capabilities[0].Name");
                issue.Message.Should()
                    .Be("The catlet capability name contains invalid characters. Only latin characters, numbers and underscores are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Networks[0].Name");
                issue.Message.Should()
                    .Be("The eryph network name contains invalid characters. Only latin characters, numbers and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Networks[0].AdapterName");
                issue.Message.Should()
                    .Be("The catlet network adapter name contains invalid characters. Only latin characters, numbers and hyphens are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("NetworkAdapters[0].Name");
                issue.Message.Should()
                    .Be("The catlet network adapter name contains invalid characters. Only latin characters, numbers and hyphens are permitted.");
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
                    .Be("The variable name contains invalid characters. Only latin characters, numbers and underscores are permitted.");
            },
            issue =>
            {
                issue.Member.Should().Be("Variables[0].Value");
                issue.Message.Should()
                    .Be("The value is not a valid boolean. Only 'true' and 'false' are allowed.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_DuplicateNames_ReturnsError()
    {
        var catletConfig = new CatletConfig()
        {
            Capabilities = 
            [
                new CatletCapabilityConfig { Name = "test_capability" },
                new CatletCapabilityConfig { Name = "TEST_CAPABILITY" },
            ],
            Drives =
            [
                new CatletDriveConfig { Name = "sda" },
                new CatletDriveConfig { Name = "SDA" },
            ],
            NetworkAdapters =
            [
                new CatletNetworkAdapterConfig { Name = "eth0" },
                new CatletNetworkAdapterConfig { Name = "ETH0" },
            ],
            Networks =
            [
                new CatletNetworkConfig { Name = "test-network" },
                new CatletNetworkConfig { Name = "Test-Network" },
            ],
            Variables =
            [
                new VariableConfig() { Name = "test_variable" },
                new VariableConfig() { Name = "Test_Variable" },
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Drives");
                issue.Message.Should().Be("The drive name 'sda' is not unique.");
            },
            issue =>
            {
                issue.Member.Should().Be("Capabilities");
                issue.Message.Should().Be("The capability name 'test_capability' is not unique.");
            },
            issue =>
            {
                issue.Member.Should().Be("Networks");
                issue.Message.Should().Be("The network name 'test-network' is not unique.");
            },
            issue =>
            {
                issue.Member.Should().Be("NetworkAdapters");
                issue.Message.Should().Be("The network adapter name 'eth0' is not unique.");
            },
            issue =>
            {
                issue.Member.Should().Be("Variables");
                issue.Message.Should().Be("The variable name 'test_variable' is not unique.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_DriveWithGenePoolSourceButNotVHD_ReturnsError()
    {
        var catletConfig = new CatletConfig()
        {
            Drives =
            [
                new CatletDriveConfig
                {
                    Name = "sda",
                    Source = "gene:acme/acme-os/1.0:my-drive",
                    Type = CatletDriveType.Phd,
                },
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Drives[0].Source");
                issue.Message.Should().Be("The drive must be plain VHD when using a gene pool source.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_DuplicateFodderNameWithDifferentSources_ReturnsSuccess()
    {
        var catletConfig = new CatletConfig()
        {
            Parent = "acme/acme-os/1.0.0",
            Fodder =
            [
                new FodderConfig()
                {
                    Name = "test-food",
                    Content = "test fodder content"
                },
                new FodderConfig()
                {
                    Name = "test-food",
                    Source = "gene:acme/acme-tools/1.0:test-fodder"
                },
                new FodderConfig()
                {
                    Name = "test-food",
                    Source = "gene:acme/other-tools/1.0:test-fodder"
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeSuccess();
    }

    [Fact]
    public void ValidateCatletConfig_DuplicateFodderNameWithSameSource_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Parent = "acme/acme-os/1.0.0",
            Fodder =
            [
                new FodderConfig()
                {
                    Name = "test-food",
                    Source = "gene:acme/acme-tools/1.0:test-fodder"
                },
                new FodderConfig()
                {
                    Name = "test-food",
                    Source = "gene:acme/acme-tools/1.0:test-fodder"
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The fodder 'gene:acme/acme-tools/1.0:test-fodder->test-food' is not unique.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_DuplicateFodderNameWithoutSource_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Parent = "acme/acme-os/1.0.0",
            Fodder =
            [
                new FodderConfig()
                {
                    Name = "test-food",
                    Content = "test fodder content",
                },
                new FodderConfig()
                {
                    Name = "test-food",
                    Content = "other test fodder content",
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The fodder 'catlet->test-food' is not unique.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_DuplicateFodderSourceWithoutName_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Parent = "acme/acme-os/1.0.0",
            Fodder =
            [
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder/1.0:my-fodder"
                },
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder/1.0:my-fodder"
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The fodder 'gene:acme/acme-fodder/1.0:my-fodder' is not unique.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_AddedFodderWithoutContentOrSource_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Fodder =
            [
                new FodderConfig()
                {
                    Name = "test-fodder",
                }
            ],
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
            Fodder =
            [
                new FodderConfig()
                {
                    Content = "test-content",
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0]");
                issue.Message.Should().Be("The name or source must be specified.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_FodderWithReferenceAndInvalidVariableBinding_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Fodder =
            [
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder/1.0:my-fodder",
                    Variables =
                    [
                        new VariableConfig()
                        {
                            Name = "testVariable",
                            Type = VariableType.Number,
                            Value = "4.2",
                            Required = true,
                            Secret = true,
                        }
                    ],
                }
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Variables[0]");
                issue.Message.Should().Be("The required flag cannot be specified when the fodder is a reference.");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Variables[0]");
                issue.Message.Should().Be("The variable type cannot be specified when the fodder is a reference.");
            });
    }

    [Fact]
    public void ValidateCatletConfig_FodderUsesGeneSetWithSingleTag_ReturnsSuccess()
    {
        var catletConfig = new CatletConfig()
        {
            Fodder =
            [
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder:first-fodder-gene",
                },
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder/latest:second-fodder-gene",
                },
                new FodderConfig()
                {
                    Name = "test-fodder",
                    Source = "gene:ACME/ACME-fodder/latest:second-fodder-gene",
                    Remove = true,
                },
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeSuccess();
    }

    [Fact]
    public void ValidateCatletConfig_FodderUsesGeneSetWithDifferentTags_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Fodder =
            [
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder:first-fodder-gene",
                },
                new FodderConfig()
                {
                    Source = "gene:acme/acme-fodder/1.0:second-fodder-gene",
                },
                new FodderConfig()
                {
                    Name = "test-fodder",
                    Source = "gene:ACME/ACME-fodder/2.0:second-fodder-gene",
                    Remove = true,
                },
                new FodderConfig()
                {
                    Source = "gene:acme/other-fodder:other-fodder-gene",
                },
                new FodderConfig()
                {
                    Source = "gene:acme/other-fodder/1.0:other-fodder-gene",
                },
            ],
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The gene set 'acme/acme-fodder' is used with different tags ('latest', '1.0', '2.0').");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The gene set 'acme/other-fodder' is used with different tags ('latest', '1.0').");
            });
    }

    [Fact]
    public void ValidateCatletConfig_MemorySizesMismatched_ReturnsFail()
    {
        var catletConfig = new CatletConfig()
        {
            Memory = new CatletMemoryConfig()
            {
                Minimum = 512,
                Startup = 256,
                Maximum = 128,
            },
        };

        var result = CatletConfigValidations.ValidateCatletConfig(catletConfig);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Memory.Maximum");
                issue.Message.Should().Be("The maximum memory must be greater than or equal to the startup memory.");
            },
            issue =>
            {
                issue.Member.Should().Be("Memory.Minimum");
                issue.Message.Should().Be("The minimum memory must be less than or equal to the startup memory.");
            },
            issue =>
            {
                issue.Member.Should().Be("Memory.Minimum");
                issue.Message.Should().Be("The minimum memory must be less than or equal to the maximum memory.");
            });
    }
}

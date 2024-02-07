using System;
using Eryph.ConfigModel.Catlets;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class BreedingTests
{
    [Fact]
    public void Child_get_parent_attributes()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Capabilities = new[]
            {
                new CatletCapabilityConfig
                {
                    Name = "Cap1"
                }
            },
            Cpu = new CatletCpuConfig { Count = 2 },
            Drives = new[]
            {
                new CatletDriveConfig
                {
                    Name = "sda",
                    Type = CatletDriveType.VHD,
                    Store = "lair",
                    Size = 100
                }
            },
            Memory = new CatletMemoryConfig { Startup = 2048 },
            NetworkAdapters = new[]
            {
                new CatletNetworkAdapterConfig
                {
                    Name = "eth0"
                }
            },
            Networks = new[]
            {
                new CatletNetworkConfig
                {
                    Name = "default",
                    AdapterName = "eth0",
                    SubnetV4 = new CatletSubnetConfig
                    {
                        Name = "sub1",
                        IpPool = "pool1"
                    }
                }
            },
            Fodder = new []{new FodderConfig
                {
                    Source = "food_from_somewhere_else"
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Project = "social",
            Environment = "env1",
            Drives = Array.Empty<CatletDriveConfig>(),
            Networks = new[]{new CatletNetworkConfig
            {
                Name = "default",
                AdapterName = "eth0"
            }},
            Fodder = new[]{new FodderConfig
            {
                Name = "food"
            }
            }
        };
        var breedChild = parent.Breed(child, "reference");

        breedChild.Parent.Should().Be("reference");
        breedChild.Project.Should().Be("social");
        breedChild.Environment.Should().Be("env1");

        breedChild.Capabilities.Should().NotBeNull();
        breedChild.Capabilities.Should().BeEquivalentTo(parent.Capabilities);
        breedChild.Capabilities.Should().NotBeSameAs(parent.Capabilities);

        breedChild.Drives.Should().NotBeNull();
        breedChild.Drives.Should().HaveCount(1);
        breedChild.Drives.Should().NotBeEquivalentTo(parent.Drives);
        breedChild.Drives?[0].Source.Should().Be("gene:reference:sda");
        breedChild.Drives.Should().NotBeSameAs(parent.Drives);

        breedChild.NetworkAdapters.Should().NotBeNull();
        breedChild.NetworkAdapters.Should().HaveCount(1);
        breedChild.NetworkAdapters.Should().BeEquivalentTo(parent.NetworkAdapters);
        breedChild.NetworkAdapters.Should().NotBeSameAs(parent.NetworkAdapters);

        breedChild.Networks.Should().NotBeNull();
        breedChild.Networks.Should().HaveCount(1);
        breedChild.Networks.Should().BeEquivalentTo(parent.Networks);
        breedChild.Networks.Should().NotBeSameAs(parent.Networks);

        breedChild.Fodder.Should().NotBeNull();
        breedChild.Fodder.Should().HaveCount(2);
        breedChild.Fodder?[0].Source.Should().Be("food_from_somewhere_else");
    }

    [Fact]
    public void Capabilities_are_merged()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Capabilities = new[]
            {
                new CatletCapabilityConfig
                {
                    Name = "Cap1",
                    Details = new []{"detail"}
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Capabilities = new[] { new CatletCapabilityConfig
            {
                Name = "Cap1",
                Details = new []{"detail2"}
            }}
        };

        var breedChild = parent.Breed(child);

        breedChild.Capabilities.Should().NotBeNull();
        breedChild.Capabilities.Should().NotBeEquivalentTo(parent.Capabilities);
        breedChild.Capabilities.Should().HaveCount(1);
        breedChild.Capabilities?[0].Details.Should().BeEquivalentTo(new[] { "detail2" });


    }

    [Fact]
    public void Drives_are_merged()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Drives = new[]
            {
                new CatletDriveConfig
                {
                    Name = "sda",
                    Type = CatletDriveType.VHD
                },
                new CatletDriveConfig
                {
                    Name = "sdb",
                    Type = CatletDriveType.VHD,
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Drives = new[]
            {
                new CatletDriveConfig
                {
                    Name = "sda",
                    Store = "none",
                },
                new CatletDriveConfig
                {
                    Name = "sdb",
                    Type = CatletDriveType.PHD,
                    Store = "none",
                    Location = "peng"
                }
            }
        };

        var breedChild = parent.Breed(child, "reference");

        breedChild.Drives.Should().NotBeNull();
        breedChild.Drives.Should().NotBeEquivalentTo(parent.Drives);
        breedChild.Drives.Should().HaveCount(2);
        breedChild.Drives?[0].Type.Should().Be(CatletDriveType.VHD);
        breedChild.Drives?[0].Store.Should().Be("none");
        breedChild.Drives?[0].Source.Should().Be("gene:reference:sda");
        breedChild.Drives?[1].Type.Should().Be(CatletDriveType.PHD);
        breedChild.Drives?[1].Store.Should().Be("none");
        breedChild.Drives?[1].Source.Should().BeNull();
        breedChild.Drives?[1].Location.Should().Be("peng");
    }

    [Fact]
    public void Drive_with_default_values_is_merged()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Drives =
            [
                new CatletDriveConfig
                {
                    Name = "sda",
                    Size = 100,
                }
            ]
        };

        var child = new CatletConfig
        {
            Name = "child", 
            Drives =
            [
                new CatletDriveConfig
                {
                    Name = "sda",
                    Type = CatletDriveType.VHD,
                }
            ]
        };

        var breedChild = parent.Breed(child, "reference");

        breedChild.Drives.Should().SatisfyRespectively(
            breedDrive =>
            {
                breedDrive.Name.Should().Be("sda");
                breedDrive.Size.Should().Be(100);
                breedDrive.Source.Should().Be("gene:reference:sda");
            });
    }
    
    [Fact]
    public void NetworkAdapters_are_merged()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            NetworkAdapters = new[]
            {
                new CatletNetworkAdapterConfig()
                {
                    Name = "sda",
                    MacAddress = "addr1"
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            NetworkAdapters = new[]
            {
                new CatletNetworkAdapterConfig()
                {
                    Name = "sda",
                    MacAddress = "addr2"
                }
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.NetworkAdapters.Should().NotBeNull();
        breedChild.NetworkAdapters.Should().NotBeEquivalentTo(parent.NetworkAdapters);
        breedChild.NetworkAdapters.Should().HaveCount(1);
        breedChild.NetworkAdapters?[0].MacAddress.Should().Be("addr2");

    }

    [Fact]
    public void Networks_are_merged()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Networks = new[]
            {
                new CatletNetworkConfig()
                {
                    Name = "sda",
                    AdapterName = "eth2",
                    SubnetV4 = new CatletSubnetConfig
                    {
                        Name = "default",
                        IpPool = "other"
                    },
                    SubnetV6 = new CatletSubnetConfig
                    {
                        Name = "default",
                        IpPool = "default"
                    }
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Networks = new[]
            {
                new CatletNetworkConfig()
                {
                    Name = "sda",
                    AdapterName = "eth1",
                    SubnetV4 = new CatletSubnetConfig
                    {
                        Name = "none-default"
                    }
                }
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.Networks.Should().NotBeNull();
        breedChild.Networks.Should().NotBeEquivalentTo(parent.Networks);
        breedChild.Networks.Should().HaveCount(1);
        breedChild.Networks?[0].AdapterName.Should().Be("eth1");
        breedChild.Networks?[0].SubnetV4.Should().NotBeNull();
        breedChild.Networks?[0].SubnetV4?.Name.Should().Be("none-default");
        breedChild.Networks?[0].SubnetV4?.IpPool.Should().BeNull();
        breedChild.Networks?[0].SubnetV6.Should().NotBeNull();
        breedChild.Networks?[0].SubnetV6?.Name.Should().Be("default");
        breedChild.Networks?[0].SubnetV6?.IpPool.Should().Be("default");

    }

    [Fact]
    public void Memory_is_merged()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Memory = new CatletMemoryConfig
            {
                Startup = 2048,
                Minimum = 1024,
                Maximum = 9096
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Memory = new CatletMemoryConfig
            {
                Startup = 2049,
                Minimum = 1025,
                Maximum = 9097
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.Memory.Should().NotBeNull();
        breedChild.Memory?.Startup.Should().Be(2049);
        breedChild.Memory?.Minimum.Should().Be(1025);
        breedChild.Memory?.Maximum.Should().Be(9097);
    }

    [Fact]
    public void Fodder_is_mixed()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "cfg",
                    Type = "type1",
                    Content = "contenta",
                    FileName = "filenamea"
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "cfg",
                    Type = "type2",
                    Content = "contentb",
                    FileName = "filenameb"
                }
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.Fodder.Should().NotBeNull();
        breedChild.Fodder.Should().NotBeEquivalentTo(parent.Fodder);
        breedChild.Fodder.Should().HaveCount(1);
        breedChild.Fodder?[0].Type.Should().Be("type2");
        breedChild.Fodder?[0].Content.Should().Be("contentb");
        breedChild.Fodder?[0].FileName.Should().Be("filenameb");

    }

    [Fact]
    public void Fodder_is_removed()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "cfg",
                    Type = "type1",
                    Content = "contenta",
                    FileName = "filenamea"
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Name = "cfg",
                    Remove = true
                }
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.Fodder.Should().NotBeNull();
        breedChild.Fodder.Should().HaveCount(0);

    }

    [Fact]
    public void Fodder_from_source_is_not_removed()
    {
        var parent = new CatletConfig
        {
            Name = "Parent",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Source = "gene:somegene/utt/123:gene1"
                }
            }
        };

        var child = new CatletConfig
        {
            Name = "child",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Source = "gene:somegene/utt/123:gene1",
                    Remove = true
                }
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.Fodder.Should().NotBeNull();
        breedChild.Fodder.Should().HaveCount(1);
        breedChild.Fodder?[0].Source.Should().Be("gene:somegene/utt/123:gene1");
        breedChild.Fodder?[0].Remove.Should().BeTrue();
    }

    [Theory]
    [InlineData(MutationType.Merge, 2)]
    [InlineData(MutationType.Remove, 1)]
    [InlineData(MutationType.Overwrite, 2)]
    public void Mutates(MutationType type, int expectedCount)
    {
        var parent = new CatletConfig
        {
            Capabilities = new[]
            {
                new CatletCapabilityConfig
                {
                    Name = "cap1",
                    Details = new []{"any"}
                },
                new CatletCapabilityConfig
                {
                    Name = "cap2"
                },
                new CatletCapabilityConfig
                {
                    Name = "cap3",
                    Mutation = MutationType.Remove
                },
            }
        };

        var child = new CatletConfig
        {
            Capabilities = new[]
            {
                new CatletCapabilityConfig
                {
                    Name = "cap1",
                    Mutation = type,
                    Details = new []{"none"}
                },
                new CatletCapabilityConfig
                {
                    Name = "cap2",
                },
            }
        };

        var breedChild = parent.Breed(child);

        breedChild.Capabilities.Should().NotBeNull();
        breedChild.Capabilities.Should().HaveCount(expectedCount);

        if (type == MutationType.Overwrite)
        {
            breedChild.Capabilities?[0].Details.Should().BeEquivalentTo(new[] { "none" });
        }
    }

}
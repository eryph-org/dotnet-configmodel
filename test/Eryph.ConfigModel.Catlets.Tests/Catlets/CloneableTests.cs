using System;
using System.Linq;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class CloneableTests
{
    private static readonly CatletConfig TestData = new()
    {
        Capabilities =
        [
            new CatletCapabilityConfig { Name = "one" }
        ],
        Cpu = new CatletCpuConfig { Count = 2 },
        Drives =
        [
            new CatletDriveConfig { Name = "sda" }
        ],
        Fodder = 
        [
            new FodderConfig
            {
                Name = "food",
                Variables =
                [
                    new VariableConfig { Name = "foodVariable" }
                ]
            }
        ],
        Memory = new CatletMemoryConfig { Startup = 1 },
        Networks =
        [
            new CatletNetworkConfig
            {
                Name = "nw",
                SubnetV4 = new CatletSubnetConfig { Name = "name" },
            }
        ],
        NetworkAdapters =
        [
            new CatletNetworkAdapterConfig { Name = "eth0" }
        ],
        Variables =
        [
            new VariableConfig { Name = "catletVariable" }
        ],
    };

    [Fact]
    public void CatletConfig_is_cloned()
    {
        var clonedConfig = TestData.Clone();

        clonedConfig.Capabilities.Should().NotBeNull();
        clonedConfig.Capabilities.Should().NotBeSameAs(TestData.Capabilities);

        clonedConfig.Cpu.Should().NotBeNull();
        clonedConfig.Cpu.Should().NotBeSameAs(TestData.Cpu);

        clonedConfig.Drives.Should().NotBeNull();
        clonedConfig.Drives.Should().NotBeSameAs(TestData.Drives);

        clonedConfig.Fodder.Should().NotBeNull();
        clonedConfig.Fodder.Should().NotBeSameAs(TestData.Fodder);

        clonedConfig.Memory.Should().NotBeNull();
        clonedConfig.Memory.Should().NotBeSameAs(TestData.Memory);

        clonedConfig.Networks.Should().NotBeNull();
        clonedConfig.Networks.Should().NotBeSameAs(TestData.Networks);

        clonedConfig.NetworkAdapters.Should().NotBeNull();
        clonedConfig.NetworkAdapters.Should().NotBeSameAs(TestData.NetworkAdapters);

        clonedConfig.Variables.Should().NotBeNull();
        clonedConfig.Variables.Should().NotBeSameAs(TestData.Variables);
    }

    [Fact]
    public void CpuConfig_is_cloned()
    {
        var clonedConfig = TestData.Cpu!.Clone();

        clonedConfig.Should().NotBeNull();
        clonedConfig.Should().NotBeSameAs(TestData.Cpu);
        clonedConfig.Should().BeEquivalentTo(TestData.Cpu);
    }

    [Fact]
    public void MemoryConfig_is_cloned()
    {
        var clonedConfig = TestData.Memory!.Clone();

        clonedConfig.Should().NotBeNull();
        clonedConfig.Should().NotBeSameAs(TestData.Memory);
        clonedConfig.Should().BeEquivalentTo(TestData.Memory);
    }

    [Fact]
    public void SubnetConfig_is_cloned()
    {
        var clonedConfig = TestData.Networks![0].SubnetV4!.Clone();

        clonedConfig.Should().NotBeNull();
        clonedConfig.Should().NotBeSameAs(TestData.Networks?[0].SubnetV4);
        clonedConfig.Should().BeEquivalentTo(TestData.Networks?[0].SubnetV4);
    }

    [Fact]
    public void Capabilities_are_cloned()
    {
        var cloned = TestData.Capabilities!
            .Select(a => a.Clone())
            .ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void NetworkAdapters_are_cloned()
    {
        var cloned = TestData.NetworkAdapters!
            .Select(a => a.Clone())
            .ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void Drives_are_cloned()
    {
        var cloned = TestData.Drives!
            .Select(a => a.Clone())
            .ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void Networks_are_cloned()
    {
        var cloned = TestData.Networks!
            .Select(a => a.Clone())
            .ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void Fodder_is_cloned()
    {
        var cloned = TestData.Fodder!
            .Select(a => a.Clone())
            .ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void FodderVariables_are_cloned()
    {
        var cloned = TestData.Fodder![0].Clone();

        cloned.Variables.Should().NotBeNull();
        cloned.Variables.Should().NotBeSameAs(TestData.Fodder![0].Variables);
    }
}
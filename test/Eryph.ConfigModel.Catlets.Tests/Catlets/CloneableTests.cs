using System;
using System.Linq;
using Eryph.ConfigModel.Catlets;
using FluentAssertions;
using Xunit;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public class CloneableTests
{
    private static readonly CatletConfig TestData = new CatletConfig
    {
        Capabilities = new[] { new CatletCapabilityConfig { Name = "one" } },
        Cpu = new CatletCpuConfig { Count = 2 },
        Memory = new CatletMemoryConfig { Startup = 1 },
        NetworkAdapters = new[] { new CatletNetworkAdapterConfig { Name = "eth0" } },
        Drives = new[] { new CatletDriveConfig { Name = "sda" } },
        Fodder = new[] { new FodderConfig { Name = "food" } },
        Networks = new[] { new CatletNetworkConfig
        {
            Name = "nw",
            SubnetV4 = new CatletSubnetConfig{Name = "name"}
        } },
    };

    [Fact]
    public void CatletConfig_is_cloned()
    {
        var clone = (TestData as ICloneable).Clone();
        clone.Should().BeOfType<CatletConfig>();
        var clonedConfig = (CatletConfig)clone;

        clonedConfig.Cpu.Should().NotBeNull();
        clonedConfig.Cpu.Should().NotBeSameAs(TestData.Cpu);

        clonedConfig.Memory.Should().NotBeNull();
        clonedConfig.Memory.Should().NotBeSameAs(TestData.Memory);

        clonedConfig.NetworkAdapters.Should().NotBeNull();
        clonedConfig.NetworkAdapters.Should().NotBeSameAs(TestData.NetworkAdapters);

        clonedConfig.Drives.Should().NotBeNull();
        clonedConfig.Drives.Should().NotBeSameAs(TestData.Drives);

        clonedConfig.Fodder.Should().NotBeNull();
        clonedConfig.Fodder.Should().NotBeSameAs(TestData.Fodder);

        clonedConfig.Fodder.Should().NotBeNull();
        clonedConfig.Fodder.Should().NotBeSameAs(TestData.Fodder);
    }

    [Fact]
    public void CpuConfig_is_cloned()
    {
        var clone = (TestData.Cpu as ICloneable)?.Clone();
        clone.Should().BeOfType<CatletCpuConfig>();
        var clonedConfig = (CatletCpuConfig)clone!;

        clonedConfig.Should().NotBeNull();
        clonedConfig.Should().NotBeSameAs(TestData.Cpu);
        clonedConfig.Should().BeEquivalentTo(TestData.Cpu);
    }

    [Fact]
    public void MemoryConfig_is_cloned()
    {
        var clone = (TestData.Memory as ICloneable)?.Clone();
        clone.Should().BeOfType<CatletMemoryConfig>();
        var clonedConfig = (CatletMemoryConfig)clone!;

        clonedConfig.Should().NotBeNull();
        clonedConfig.Should().NotBeSameAs(TestData.Memory);
        clonedConfig.Should().BeEquivalentTo(TestData.Memory);
    }

    [Fact]
    public void SubnetConfig_is_cloned()
    {
        var clone = (TestData.Networks?[0].SubnetV4 as ICloneable)?.Clone();
        clone.Should().BeOfType<CatletSubnetConfig>();
        var clonedConfig = (CatletSubnetConfig)clone!;

        clonedConfig.Should().NotBeNull();
        clonedConfig.Should().NotBeSameAs(TestData.Networks?[0].SubnetV4);
        clonedConfig.Should().BeEquivalentTo(TestData.Networks?[0].SubnetV4);
    }

    [Fact]
    public void Capabilities_are_cloned()
    {
        var cloned = TestData.Capabilities?
            .Select(a => (a as ICloneable).Clone())
            .Cast<CatletCapabilityConfig>().ToArray();


        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void NetworkAdapters_are_cloned()
    {
        var cloned = TestData.NetworkAdapters?
            .Select(a => (a as ICloneable).Clone())
            .Cast<CatletNetworkAdapterConfig>().ToArray();


        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void Drives_are_cloned()
    {
        var cloned = TestData.Drives?
            .Select(a => (a as ICloneable).Clone())
            .Cast<CatletDriveConfig>().ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void Networks_are_cloned()
    {
        var cloned = TestData.Networks?
            .Select(a => (a as ICloneable).Clone())
            .Cast<CatletNetworkConfig>().ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }

    [Fact]
    public void Fodder_is_cloned()
    {
        var cloned = TestData.Fodder?
            .Select(a => (a as ICloneable).Clone())
            .Cast<FodderConfig>().ToArray();

        cloned.Should().NotBeNull();
        cloned.Should().HaveCount(1);
    }
}
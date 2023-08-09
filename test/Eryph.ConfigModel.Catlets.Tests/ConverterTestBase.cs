using Eryph.ConfigModel.Catlets;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests;

public class ConverterTestBase
{
    protected static void AssertSample1(CatletConfig config)
    {
        config.Name.Should().Be("cinc-windows");
        config.Society.Should().Be("cinc");
        config.Environment.Should().Be("env1");

        config.VCatlet.Should().NotBeNull();
        config.VCatlet.Parent.Should().Be("dbosoft/winsrv2019-standard/20220324");
        config.VCatlet.Slug.Should().Be("cinc-slug");
        config.VCatlet.Shelter.Should().Be("ds1");
        config.VCatlet.Cpu.Should().NotBeNull();
        config.VCatlet.Cpu.Count.Should().Be(4);

        config.VCatlet.Drives.Should().HaveCount(1);
        config.VCatlet.Drives[0].Name.Should().Be("data");
        config.VCatlet.Drives[0].Size.Should().Be(1);
        config.VCatlet.Drives[0].Shelter.Should().Be("ds2");
        config.VCatlet.Drives[0].Slug.Should().Be("cinc-shared");
        config.VCatlet.Drives[0].Parent.Should().Be("some_template");
        config.VCatlet.Drives[0].Type.Should().Be(VirtualCatletDriveType.SharedVHD);

        config.VCatlet.NetworkAdapters.Should().HaveCount(2);
        config.VCatlet.NetworkAdapters[0].Name.Should().Be("eth0");
        config.VCatlet.NetworkAdapters[0].MacAddress.Should().Be("4711");
        config.VCatlet.NetworkAdapters[1].Name.Should().Be("eth1");
        config.VCatlet.NetworkAdapters[1].MacAddress.Should().Be("4712");

        config.VCatlet.Memory.Should().NotBeNull();
        config.VCatlet.Memory.Startup.Should().Be(1024);
        config.VCatlet.Memory.Minimum.Should().Be(512);
        config.VCatlet.Memory.Maximum.Should().Be(4096);

        config.VCatlet.Capabilities.Should().NotBeNull();
        config.VCatlet.Capabilities.Should().HaveCount(2);
        config.VCatlet.Capabilities[0].Name.Should().Be("nested_virtualization");
        config.VCatlet.Capabilities[1].Name.Should().Be("secure_boot");
        config.VCatlet.Capabilities[1].Details.Should().NotBeNull();
        config.VCatlet.Capabilities[1].Details.Should().HaveCount(2);
        config.VCatlet.Capabilities[1].Details[0].Should().Be("tpm");
        config.VCatlet.Capabilities[1].Details[1].Should().Be("shielded");

        config.Networks.Should().HaveCount(2);
        config.Networks[0].Name.Should().Be("default");
        config.Networks[0].AdapterName.Should().Be("eth0");
        config.Networks[0].Name.Should().Be("default");
        config.Networks[0].AdapterName.Should().Be("eth0");
        config.Networks[0].SubnetV4.Should().NotBeNull();
        config.Networks[0].SubnetV4.Name.Should().Be("other");
        config.Networks[0].SubnetV4.IpPool.Should().Be("other_pool");
        config.Networks[1].Name.Should().Be("backup");
        config.Networks[1].AdapterName.Should().Be("eth1");
        config.Networks[1].SubnetV4.Should().BeNull();
        config.Networks[1].SubnetV6.Should().BeNull();

        config.SocialName.Should().Be("cinc-host");
        config.Fodder.Should().NotBeNull();
        config.Fodder.Should().HaveCount(1);
        config.Fodder[0].Name.Should().Be("admin-windows");
        config.Fodder[0].Type.Should().Be("cloud-config");
        config.Fodder[0].FileName.Should().Be("filename");
        config.Fodder[0].Secret.Should().Be(true);
        config.Fodder[0].Content.Should().Contain("- name: Admin");
        config.Fodder[0].Content.Should().NotEndWith("\0");
    }
}
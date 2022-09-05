using FluentAssertions;

namespace Eryph.ConfigModel.Machine.Tests;

public class ConverterTestBase
{
    protected static void AssertSample1(MachineConfig config)
    {
        config.Name.Should().Be("cinc-windows");
        config.Project.Should().Be("cinc");
        config.Environment.Should().Be("env1");

        config.VM.Should().NotBeNull();
        config.VM.Image.Should().Be("dbosoft/winsrv2019-standard/20220324");
        config.VM.Slug.Should().Be("cinc-slug");
        config.VM.DataStore.Should().Be("ds1");
        config.VM.Cpu.Should().NotBeNull();
        config.VM.Cpu.Count.Should().Be(4);

        config.VM.Drives.Should().HaveCount(1);
        config.VM.Drives[0].Name.Should().Be("data");
        config.VM.Drives[0].Size.Should().Be(1);
        config.VM.Drives[0].DataStore.Should().Be("ds2");
        config.VM.Drives[0].Slug.Should().Be("cinc-shared");
        config.VM.Drives[0].Template.Should().Be("some_template");
        config.VM.Drives[0].Type.Should().Be(VirtualMachineDriveType.SharedVHD);

        config.VM.NetworkAdapters.Should().HaveCount(1);
        config.VM.NetworkAdapters[0].Name.Should().Be("eth1");
        config.VM.NetworkAdapters[0].MacAddress.Should().Be("4711");

        config.VM.Memory.Should().NotBe(null);
        config.VM.Memory.Startup.Should().Be(1024);
        config.VM.Memory.Minimum.Should().Be(512);
        config.VM.Memory.Maximum.Should().Be(4096);


        config.Networks.Should().HaveCount(1);
        config.Networks[0].Name.Should().Be("lan");
        config.Networks[0].AdapterName.Should().Be("eth2");

        config.Provisioning.Should().NotBeNull();
        config.Provisioning.Hostname.Should().Be("cinc-host");
        config.Provisioning.Config.Should().HaveCount(1);
        config.Provisioning.Config[0].Name.Should().Be("admin-windows");
        config.Provisioning.Config[0].Type.Should().Be("cloud-config");
        config.Provisioning.Config[0].FileName.Should().Be("filename");
        config.Provisioning.Config[0].Sensitive.Should().Be(true);
        config.Provisioning.Config[0].Content.Should().Contain("- name: Admin");
        config.Provisioning.Config[0].Content.Should().NotEndWith("\0");
    }
}
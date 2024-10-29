using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public abstract class CatletConfigSerializerTestBase
{
    protected static void AssertSample1(CatletConfig config)
    {
        config.Should().NotBeNull();

        config.Name.Should().Be("cinc-windows");
        config.Project.Should().Be("homeland");
        config.Environment.Should().Be("world");
        
        config.Parent.Should().Be("dbosoft/winsrv2019-standard/20220324");
        config.Location.Should().Be("cinc");
        config.Store.Should().Be("home");
        config.Cpu.Should().NotBeNull();
        config.Cpu!.Count.Should().Be(4);

        config.Drives.Should().HaveCount(1);
        config.Drives?[0].Name.Should().Be("data");
        config.Drives?[0].Size.Should().Be(1);
        config.Drives?[0].Store.Should().Be("ds2");
        config.Drives?[0].Location.Should().Be("cinc-shared");
        config.Drives?[0].Source.Should().Be("some_template");
        config.Drives?[0].Type.Should().Be(CatletDriveType.SharedVHD);
        config.Drives?[0].Mutation.Should().Be(MutationType.Overwrite);

        config.NetworkAdapters.Should().HaveCount(2);
        config.NetworkAdapters?[0].Name.Should().Be("eth0");
        config.NetworkAdapters?[0].MacAddress.Should().Be("4711");
        config.NetworkAdapters?[1].Name.Should().Be("eth1");
        config.NetworkAdapters?[1].MacAddress.Should().Be("4712");
        config.NetworkAdapters?[1].Mutation.Should().Be(MutationType.Remove);

        config.Memory.Should().NotBeNull();
        config.Memory?.Startup.Should().Be(1024);
        config.Memory?.Minimum.Should().Be(512);
        config.Memory?.Maximum.Should().Be(4096);

        config.Capabilities.Should().NotBeNull();
        config.Capabilities.Should().HaveCount(2);
        config.Capabilities?[0].Name.Should().Be("nested_virtualization");
        config.Capabilities?[1].Name.Should().Be("secure_boot");
        config.Capabilities?[1].Details.Should().NotBeNull();
        config.Capabilities?[1].Details.Should().HaveCount(2);
        config.Capabilities?[1].Details?[0].Should().Be("tpm");
        config.Capabilities?[1].Details?[1].Should().Be("shielded");

        config.Networks.Should().HaveCount(2);
        config.Networks?[0].Name.Should().Be("default");
        config.Networks?[0].AdapterName.Should().Be("eth0");
        config.Networks?[0].Name.Should().Be("default");
        config.Networks?[0].AdapterName.Should().Be("eth0");
        config.Networks?[0].SubnetV4.Should().NotBeNull();
        config.Networks?[0].SubnetV4?.Name.Should().Be("other");
        config.Networks?[0].SubnetV4?.IpPool.Should().Be("other_pool");
        config.Networks?[1].Name.Should().Be("backup");
        config.Networks?[1].AdapterName.Should().Be("eth1");
        config.Networks?[1].SubnetV4.Should().BeNull();
        config.Networks?[1].SubnetV6.Should().BeNull();

        config.Hostname.Should().Be("cinc-host");
        config.Fodder.Should().NotBeNull();
        config.Fodder.Should().HaveCount(2);
        config.Fodder?[0].Name.Should().Be("first");
        config.Fodder?[1].Name.Should().Be("admin-windows");
        config.Fodder?[1].Type.Should().Be("cloud-config");
        config.Fodder?[1].FileName.Should().Be("filename");
        config.Fodder?[1].Secret.Should().Be(true);
        config.Fodder?[1].Content.Should().Contain("- name: Admin");
        config.Fodder?[1].Content.Should().NotEndWith("\0");

        config.Variables.Should().SatisfyRespectively(
            variable =>
            {
                variable.Name.Should().Be("first");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("first value");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            },
            variable =>
            {
                variable.Name.Should().Be("second");
                variable.Type.Should().Be(VariableType.Number);
                variable.Value.Should().Be("-4.2");
                variable.Required.Should().BeTrue();
                variable.Secret.Should().BeTrue();
            });
    }

    protected static void AssertNativeVariableValuesSample(CatletConfig config)
    {
        config.Fodder.Should().SatisfyRespectively(
            fodder => fodder.Variables.Should().SatisfyRespectively(
                variable =>
                {
                    variable.Name.Should().Be("boolean");
                    variable.Type.Should().BeNull();
                    variable.Value.Should().Be("true");
                    variable.Required.Should().BeNull();
                    variable.Secret.Should().BeNull();
                },
                variable =>
                {
                    variable.Name.Should().Be("number");
                    variable.Type.Should().BeNull();
                    variable.Value.Should().Be("-4.2");
                    variable.Required.Should().BeNull();
                    variable.Secret.Should().BeNull();
                }));

        config.Variables.Should().SatisfyRespectively(
            variable =>
            {
                variable.Name.Should().Be("boolean");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("true");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            },
            variable =>
            {
                variable.Name.Should().Be("number");
                variable.Type.Should().BeNull();
                variable.Value.Should().Be("-4.2");
                variable.Required.Should().BeNull();
                variable.Secret.Should().BeNull();
            });
    }
}
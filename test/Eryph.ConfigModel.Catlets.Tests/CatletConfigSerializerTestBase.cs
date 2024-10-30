using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests;

public abstract class CatletConfigSerializerTestBase
{
    protected static void AssertComplexConfig(CatletConfig config)
    {
        config.Should().NotBeNull();

        config.Name.Should().Be("cinc-windows");
        config.Project.Should().Be("homeland");
        config.Environment.Should().Be("world");

        config.Hostname.Should().Be("cinc-host");

        config.Parent.Should().Be("dbosoft/winsrv2019-standard/20220324");
        config.Location.Should().Be("cinc");
        config.Store.Should().Be("home");
        
        config.Cpu.Should().NotBeNull();
        config.Cpu!.Count.Should().Be(4);

        config.Capabilities.Should().SatisfyRespectively(
            capability =>
            {
                capability.Name.Should().Be("nested_virtualization");
                capability.Details.Should().BeNull();
                capability.Mutation.Should().BeNull();
            },
            capability =>
            {
                capability.Name.Should().Be("secure_boot");
                capability.Details.Should().SatisfyRespectively(
                    detail => detail.Should().Be("tpm"),
                    detail => detail.Should().Be("shielded"));
                capability.Mutation.Should().Be(MutationType.Remove);
            });

        config.Drives.Should().SatisfyRespectively(
            drive =>
            {
                drive.Name.Should().Be("data");
                drive.Size.Should().Be(1);
                drive.Store.Should().Be("ds2");
                drive.Location.Should().Be("cinc-shared");
                drive.Source.Should().Be("some_template");
                drive.Type.Should().Be(CatletDriveType.SharedVHD);
                drive.Mutation.Should().Be(MutationType.Overwrite);
            });

        config.Memory.Should().NotBeNull();
        config.Memory!.Startup.Should().Be(1024);
        config.Memory.Minimum.Should().Be(512);
        config.Memory.Maximum.Should().Be(4096);

        config.NetworkAdapters.Should().SatisfyRespectively(
            adapter =>
            {
                adapter.Name.Should().Be("eth0");
                adapter.MacAddress.Should().Be("4711");
                adapter.Mutation.Should().BeNull();
            },
            adapter =>
            {
                adapter.Name.Should().Be("eth1");
                adapter.MacAddress.Should().Be("4712");
                adapter.Mutation.Should().Be(MutationType.Remove);
            });

        config.Networks.Should().SatisfyRespectively(
            network =>
            {
                network.Name.Should().Be("default");
                network.AdapterName.Should().Be("eth0");
                network.SubnetV4.Should().NotBeNull();
                network.SubnetV4!.Name.Should().Be("other");
                network.SubnetV4.IpPool.Should().Be("other_pool");
                network.SubnetV6.Should().NotBeNull();
                network.Mutation.Should().BeNull();
            },
            network =>
            {
                network.Name.Should().Be("backup");
                network.AdapterName.Should().Be("eth1");
                network.SubnetV4.Should().BeNull();
                network.SubnetV6.Should().BeNull();
                network.Mutation.Should().BeNull();
            });

        config.Fodder.Should().SatisfyRespectively(
            fodder =>
            {
                fodder.Name.Should().Be("first");
                fodder.Type.Should().BeNull();
                fodder.FileName.Should().BeNull();
                fodder.Remove.Should().BeNull();
                fodder.Secret.Should().BeNull();
                fodder.Source.Should().BeNull();
                fodder.Content.Should().BeNull();
                fodder.Variables.Should().BeNull();
            },
            fodder =>
            {
                fodder.Name.Should().Be("admin-windows");
                fodder.Type.Should().Be("cloud-config");
                fodder.FileName.Should().Be("filename");
                fodder.Remove.Should().BeNull();
                fodder.Secret.Should().Be(true);
                fodder.Source.Should().BeNull();
                fodder.Content.Should().Be("users:\n  - name: Admin\n    groups: [ \"Administrators\" ]\n    passwd: \"{{password}}\"\n");
                fodder.Content.Should().NotEndWith("\0");
                fodder.Variables.Should().SatisfyRespectively(
                    variable =>
                    {
                        variable.Name.Should().Be("password");
                        variable.Type.Should().Be(VariableType.String);
                        variable.Value.Should().Be("InitialPassw0rd");
                        variable.Required.Should().BeTrue();
                        variable.Secret.Should().BeTrue();
                    });
            });

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
}

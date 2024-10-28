using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ConfigModel.Catlets;
using FluentAssertions;

namespace Eryph.ConfigModel.Catlet.Tests.Catlets;

public abstract class SerializerTestBase
{
    protected void ShouldBeSimpleConfig(CatletConfig catletConfig)
    {
        catletConfig.Should().NotBeNull();

        catletConfig.Cpu.Should().NotBeNull();
        catletConfig.Cpu!.Count.Should().Be(2);

        catletConfig.Memory.Should().NotBeNull();
        catletConfig.Memory!.Startup.Should().Be(1024);
        catletConfig.Memory.Minimum.Should().BeNull();
        catletConfig.Memory.Maximum.Should().BeNull();
    }
}

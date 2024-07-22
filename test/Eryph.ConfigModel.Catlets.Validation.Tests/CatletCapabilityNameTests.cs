using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class CatletCapabilityNameTests
{
    [Theory]
    [InlineData("secure_boot")]
    [InlineData("nested_virtualization")]
    public void NewValidation_ValidCatletCapabilityName_ReturnsSuccess(
        string capabilityName)
    {
        CatletCapabilityName.NewValidation(capabilityName)
            .Should().BeSuccess().Which.Value
            .Should().Be(capabilityName.ToLowerInvariant());
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class CatletNetworkAdapterNameTests
{
    [Theory]
    [InlineData("eth0")]
    [InlineData("ETH0")]
    public void NewValidation_ValidCatletNetworkAdapterName_ReturnsSuccess(
        string adapterName)
    {
        CatletNetworkAdapterName.NewValidation(adapterName)
            .Should().BeSuccess().Which.Value
            .Should().Be(adapterName.ToLowerInvariant());
    }

    [Theory]
    [InlineData("eth/0")]
    [InlineData("eth 0")]
    public void NewValidation_NameWithInvalidCharacters_ReturnsFail(
        string adapterName)
    {
        CatletNetworkAdapterName.NewValidation(adapterName)
            .Should().BeFail().Which.Should().SatisfyRespectively(
                error => error.Message.Should().Be(
                    "The catlet network adapter name contains invalid characters. Only latin characters, numbers and hyphens are permitted."));
    }


    [Fact]
    public void NewValidation_TooLongName_ReturnsFail()
    {
        string adapterName = new('a', 16);

        CatletNetworkAdapterName.NewValidation(adapterName)
            .Should().BeFail().Which.Should().SatisfyRespectively(
                error => error.Message.Should().Be(
                    $"The catlet network adapter name is longer than the maximum length of 15 characters."));
    }
}

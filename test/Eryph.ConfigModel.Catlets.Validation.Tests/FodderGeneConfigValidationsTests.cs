﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Variables;
using static Eryph.ConfigModel.FodderGeneConfigValidations;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class FodderGeneConfigValidationsTests
{
    [Fact]
    public void ValidateFodderGeneConfig_FodderWithUnsupportedData_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig
                {
                    Name = "test-fodder",
                    Source = "gene:acme/acme-linux/latest:fodder",
                    Variables = new[]
                    {
                        new VariableConfig
                        {
                            Name = "test-variable",
                        },
                    },
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Source");
                issue.Message.Should().Be("References are not supported in fodder genes. The source must be empty.");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Variables");
                issue.Message.Should().Be("Variables are not supported here.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_DataIsMissing_ReturnsFail()
    {
        var config = new FodderGeneConfig();

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Name");
                issue.Message.Should().Be("The Name is required.");
            },
            issue =>
            {
                issue.Member.Should().Be("Fodder");
                issue.Message.Should().Be("The list must have 1 or more entries.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_AddedFodderWithoutContent_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig
                {
                    Name = "test-fodder",
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0]");
                issue.Message.Should().Be("The content must be specified when adding fodder.");
            });
    }

    [Fact]
    public void ValidateFodderGeneConfig_FodderWithoutName_ReturnsFail()
    {
        var config = new FodderGeneConfig
        {
            Name = "test",
            Fodder = new[]
            {
                new FodderConfig()
                {
                    Content = "test-content",
                },
            },
        };

        var result = ValidateFodderGeneConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Member.Should().Be("Fodder[0].Name");
                issue.Message.Should().Be("The Name is required.");
            });
    }
}

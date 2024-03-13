﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class StorageIdentifierTests
{
    [Theory]
    [InlineData("be8buperxp1c")]
    [InlineData("BE8BUPERXP1C")]
    public void Validate_AutoGeneratedId_ReturnsSuccess(string id)
    {
        var result = StorageIdentifier.NewValidation(id);

        result.Should().BeSuccess()
            .Which.Value.Should().Be("be8buperxp1c");
    }
}

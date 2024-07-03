using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class ValidationsTests
{
    [Fact]
    public void ValidateDistinct_DuplicateKeys_ReturnsFail()
    {
        var items = new[]
        {
            new TestData("John", "Doe", 42),
            new TestData("Jane", "Doe", 41),
            new TestData("Alice", "Adler", 52),
            new TestData("John", "Doe", 43),
            new TestData("Jane", "Doe", 42),
        };

        var result = Validations.ValidateDistinct<TestData, TestDataKey>(
            items, data => new TestDataKey(data.FirstName, data.LastName), "full name");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should().Be("The full name 'John Doe' is not unique."),
            error => error.Message.Should().Be("The full name 'Jane Doe' is not unique."));
    }

    [Fact]
    public void ValidateDistinct_InvalidKeys_ReturnsFail()
    {
        var items = new[]
        {
            new TestData("John", "Doe", 42),
            new TestData("Jane", "Doe", 41),
        };

        var result = Validations.ValidateDistinct<TestData, TestDataKey>(
            items, data => Error.New($"The first name '{data.FirstName}' is invalid."), "full name");

        
        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error =>
            {
                error.Message.Should().Be("Cannot create the full name.");
                error.Inner.Should().BeSome().Which.Message
                    .Should().Be("The first name 'John' is invalid.");
            },
            error =>
            {
                error.Message.Should().Be("Cannot create the full name.");
                error.Inner.Should().BeSome().Which.Message
                    .Should().Be("The first name 'Jane' is invalid.");
            });
    }

    private sealed class TestData(string firstName, string lastName, int age)
    {
        public string FirstName { get; } = firstName;

        public string LastName { get; } = lastName;
       
        public int Age { get; } = age;
    };

    private sealed record TestDataKey(string FirstName, string LastName)
    {
        public override string ToString() => $"{FirstName} {LastName}";
    };

    [Theory]
    [InlineData("some..test")]
    [InlineData("some--test")]
    [InlineData("some__test")]
    [InlineData("some  test")]
    public void ValidateCharacters_ConsecutiveSpecialCharacters_ReturnsFail(string value)
    {
        var result = Validations.ValidateCharacters(value, "value", true, true, true, true);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The value cannot contain consecutive dots, hyphens, underscores or spaces."));
    }

    [Theory]
    [InlineData(".test")]
    [InlineData("-test")]
    [InlineData("_test")]
    [InlineData(" test")]
    public void ValidateCharacters_SpecialCharacterInTheBeginning_ReturnsFail(string value)
    {
        var result = Validations.ValidateCharacters(value, "value", true, true, true, true);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The value cannot start with a dot, hyphen, underscore or space."));
    }

    [Theory]
    [InlineData("test.")]
    [InlineData("test-")]
    [InlineData("test_")]
    [InlineData("test ")]
    public void ValidateCharacters_SpecialCharacterAtTheEnd_ReturnsFail(string value)
    {
        var result = Validations.ValidateCharacters(value, "value", true, true, true, true);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The value cannot end with a dot, hyphen, underscore or space."));
    }

    [Theory]
    [InlineData(@"Z:\")]
    [InlineData(@"Z:\abc\def")]
    [InlineData(@"Z:\abc\def\")]
    [InlineData(@"\\TESTSRV\abc\def\")]
    public void ValidateWindowsPath_ValidPath_ReturnsSuccess(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeSuccess().Which.Should().Be(path);
    }

    [Theory]
    [InlineData("not|a|path")]
    [InlineData("not?a?path")]
    [InlineData("not*a*path")]
    [InlineData("not\"a\"path")]
    [InlineData("not<a>path")]
    public void ValidateWindowsPath_PathWithInvalidCharacters_ReturnsFail(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must be a valid Windows path but contains invalid characters."));
    }

    [Theory]
    [InlineData(@"abc\def")]
    [InlineData(@"Z:abc\def")]
    public void ValidateWindowsPath_NotRooted_ReturnsFail(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must be a fully-qualified path."));
    }

    [Theory]
    [InlineData(@"Z:\abc\.\def")]
    [InlineData(@"Z:\abc\..\def")]
    public void ValidateWindowsPath_ContainsRelativeSegments_ReturnsFail(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must be a path without relative segments."));
    }

    [Fact]
    public void ValidateWindowsPath_ContainsUnixSeparator_ReturnsFail()
    {
        var result = Validations.ValidateWindowsPath(@"Z:\abc/def", "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must only contain Windows directory separators."));
    }
}

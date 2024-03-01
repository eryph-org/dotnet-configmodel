using FluentAssertions;
using FluentAssertions.LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;

namespace Eryph.ConfigModel.Catlets.Validation.Tests
{
    public class ValidationsTests
    {

        [Fact]
        public void Test1()
        {
            var result = Validations<ProjectName>.ValidateCharacters("a..--b");

            var errors = result.Should().BeFail().Subject;

            errors.Should().HaveCount(2);
        }


        [Fact]
        public void Test2()
        {
            var projectName = ProjectName.Validate("my--project");

            var option = ProjectName.NewOption("my-project");

            var errors = projectName.Should().BeSuccess().Which;

            errors.Value.Should().Be("my-project");
        }

        [Fact]
        public void Test3()
        {
            var error = Error.New("My error");
            var tried = Prelude.Try<string>(() => throw error.ToErrorException());

            var projectName = ProjectName.Validate("my--project").ToEither().ToValidation().ToEither().ToValidation();

            var option = ProjectName.NewOption("my-project");

            var errors = projectName.Should().BeSuccess().Which;

            errors.Value.Should().Be("my-project");
        }
    }
}

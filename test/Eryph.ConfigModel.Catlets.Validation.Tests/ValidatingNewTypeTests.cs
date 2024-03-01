using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.LanguageExt;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel.Catlets.Validation.Tests
{
    public class ValidatingNewTypeTests
    {
        public class TestType : ValidatingNewType<TestType, string, OrdStringOrdinalIgnoreCase>
        {
            public TestType(string value) : base(value)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        public class TestType2 : ValidatingNewType<TestType2, string, OrdStringOrdinalIgnoreCase>
        {
            public TestType2(string value) : base(value)
            {
                var result = Fail<Error, string>(Error.New("First validation failed"))
                    | Fail<Error, string>(Error.New("Second validation failed"));

                result.ToEither().MapLeft(Error.Many).IfLeft(e => e.ToErrorException().Rethrow());
            }
        }

        [Fact]
        public void Test1()
        {
            var result = TestType.Validate("test");

            var errors = result.Should().BeFail().Subject;

            errors.Should().HaveCount(1);
        }

        [Fact]
        public void Test2()
        {
            var result = TestType2.Validate("test");

            var errors = result.Should().BeFail().Subject;

            errors.Should().HaveCount(1);
        }
    }
}

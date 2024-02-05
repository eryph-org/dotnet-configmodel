using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eryph.ConfigModel.Converters;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Core.Tests
{
    public class DictionaryConverterBaseTests
    {
        [Theory]
        [InlineData("StringValue")]
        [InlineData("stringvalue")]
        [InlineData("STRINGVALUE")]
        [InlineData("string_value")]
        public void ConvertFromDictionary_CompatiblePropertyName_ReturnsValue(
            string propertyName)
        {
            const string value = "lorem ipsum";
            var dictionary = new Dictionary<object, object>
            {
                [propertyName] = value,
            };

            var result = ConvertFromDictionary(dictionary);

            result.Should().NotBeNull();
            result!.StringValue.Should().Be(value);

        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(true, true)]
        [InlineData("true", true)]
        [InlineData("True", true)]
        public void ConvertFromDictionary_ValidBooleanValue_ReturnsBoolean(
            object? value,
            bool? expected)
        {
            var dictionary = new Dictionary<object, object>
            {
                ["BooleanValue"] = value!,
            };

            var result = ConvertFromDictionary(dictionary);
            result.Should().NotBeNull();
            result!.BooleanValue.Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("lorem")]
        public void ConvertFromDictionary_InvalidBooleanValue_ThrowsException(
            object? value)
        {
            var dictionary = new Dictionary<object, object>
            {
                ["BooleanValue"] = value!,
            };

            FluentActions.Invoking(() => ConvertFromDictionary(dictionary))
                .Should().Throw<InvalidConfigModelException>()
                .Which.Message.Should().Contain("BooleanValue");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(1, 1)]
        [InlineData("1", 1)]
        public void ConvertFromDictionary_ValidIntegerValue_ReturnsInteger(
            object? value,
            int? expected)
        {
            var dictionary = new Dictionary<object, object>
            {
                ["IntegerValue"] = value!,
            };

            var result = ConvertFromDictionary(dictionary);
            result.Should().NotBeNull();
            result!.IntegerValue.Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(1.5)]
        [InlineData("1.5")]
        public void ConvertFromDictionary_InvalidIntegerValue_ThrowsException(
            object? value)
        {
            var dictionary = new Dictionary<object, object>
            {
                ["IntegerValue"] = value!,
            };

            FluentActions.Invoking(() => ConvertFromDictionary(dictionary))
                .Should().Throw<InvalidConfigModelException>()
                .Which.Message.Should().Contain("IntegerValue");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("lorem ipsum", "lorem ipsum")]
        public void ConvertFromDictionary_ValidStringValue_ReturnsString(
            object? value,
            string? expected)
        {
            var dictionary = new Dictionary<object, object>
            {
                ["StringValue"] = value!,
            };

            var result = ConvertFromDictionary(dictionary);
            
            result.Should().NotBeNull();
            result!.StringValue.Should().Be(expected);
        }

        [Fact]
        public void ConvertFromDictionary_ValidYaml_ReturnsValidObject()
        {
            string yaml = """
                          BooleanValue: true
                          IntegerValue: 42
                          StringValue: lorem ipsum
                          """;

            var deserializer = new DeserializerBuilder().Build();
            var dictionary = deserializer.Deserialize<Dictionary<object, object>>(yaml);

            var result = ConvertFromDictionary(dictionary.ToDictionary(kv => (object)kv.Key, kv => kv.Value));
            
            result.Should().NotBeNull();
            result!.BooleanValue.Should().BeTrue();
            result.IntegerValue.Should().Be(42);
            result.StringValue.Should().Be("lorem ipsum");
        }

        [Fact]
        public void ConvertFromDictionary_ValidJson_ReturnsValidObject()
        {
            string json = """
                          {
                            "BooleanValue": true,
                            "IntegerValue": 42,
                            "StringValue": "lorem ipsum"
                          }
                          """;

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object?>>(json)
                ?.ToDictionary(kv => (object)kv.Key, kv => kv.Value);

            dictionary.Should().NotBeNull();

            var result = ConvertFromDictionary(dictionary!);
            
            result.Should().NotBeNull();
            result!.BooleanValue.Should().BeTrue();
            result.IntegerValue.Should().Be(42);
            result.StringValue.Should().Be("lorem ipsum");
        }

        private static TestModel? ConvertFromDictionary(IDictionary<object, object> dictionary)
        {
            var context = new ConverterContext<TestModel>(new DictionaryConverterProvider<TestModel>(Array.Empty<IDictionaryConverter<TestModel>>()));
            var converter = new TestModelDictionaryConverter();

            return converter.ConvertFromDictionary(context, dictionary);
        }

        private class TestModelDictionaryConverter : DictionaryConverterBase<TestModel, TestModel>
        {
            public override TestModel? ConvertFromDictionary(
                IConverterContext<TestModel> context,
                IDictionary<object, object> dictionary,
                object? data = default)
            {
                return new TestModel()
                {
                    BooleanValue = GetBoolProperty(dictionary, nameof(TestModel.BooleanValue)),
                    IntegerValue = GetIntProperty(dictionary, nameof(TestModel.IntegerValue)),
                    StringValue = GetStringProperty(dictionary, nameof(TestModel.StringValue)),
                };
            }
        }

        private class TestModel
        {
            public bool? BooleanValue { get; init; }

            public int? IntegerValue { get; init; }

            public string? StringValue { get; init; }
        }
    }
}

using System;
using FluentAssertions;
using NUnit.Framework;
using SimpleAmazonSQS.Converters;

namespace SimpleAmazonSQS.Tests.Converters
{
    [TestFixture]
    public class ValueTypeConverterTests
    {
        const string GuidString = "089d40bd-bf86-4052-bfb1-bf32328889bb";

        [Test]
        public void ConvertFromString_GivenGuid_ShouldConvert()
        {
            var converter = new ValueTypeConverter<Guid>();
            var guid = (Guid)converter.ConvertFromString(GuidString);
            guid
                .Should()
                .Be(new Guid(GuidString));
        }

        [Test]
        public void ConvertToString_GivenGuid_ShouldConvert()
        {
            var converter = new ValueTypeConverter<Guid>();
            var guidString = converter.ConvertToString(new Guid(GuidString));

            guidString
                .Should()
                .Be(GuidString);
        }
        
        [TestCase(typeof(int), "10", 10)]
        [TestCase(typeof(float), "10.1", 10.1f)]
        public void ConvertFromString_GivenType_ShouldConvert(Type type, string stringValue, ValueType expectedValue)
        {
            var converter = CreateValueTypeConverter(type);
            
            var value = converter.ConvertFromString(stringValue);
            value
                .Should()
                .Be(expectedValue);
        }

        [TestCase(typeof(int), 10, "10")]
        [TestCase(typeof(float), 10.1f, "10.1")]
        public void ConvertToString_GivenType_ShouldConvert(Type type, ValueType value, string expectedValue)
        {
            var converter = CreateValueTypeConverter(type);
            converter
                .ConvertToString(value)
                .Should()
                .Be(expectedValue);
        }
        
        private IConverter CreateValueTypeConverter(Type type)
        {
            var valueTypeConverterType = typeof (ValueTypeConverter<>).MakeGenericType(type);
            return (IConverter)Activator.CreateInstance(valueTypeConverterType);
        }
    }
}

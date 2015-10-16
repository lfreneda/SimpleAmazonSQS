using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SimpleAmazonSQS.Converters;

namespace SimpleAmazonSQS.Tests.Converters
{
    [TestFixture]
    public class ConverterFactoryTests
    {
        private IConverterFactory _converterFactory;

        [TestCase(typeof(int))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(float))]
        [TestCase(typeof(float?))]
        public void Create_GivenValueType_ShouldCreateValueTypeConverter(Type type)
        {
            CreateConverterFactory(type);

            _converterFactory
                .Create()
                .GetType()
                .GetGenericTypeDefinition()
                .Should()
                .Be(typeof (ValueTypeConverter<>));
        }

        [TestCase(typeof(string))]
        [TestCase(typeof(FakePerson))]
        [TestCase(typeof(List<int>))]
        public void Create_GivenReferenceType_ShouldCreateReferenceTypeConverter(Type type)
        {
            CreateConverterFactory(type);

            _converterFactory
                .Create()
                .GetType()
                .GetGenericTypeDefinition()
                .Should()
                .Be(typeof(ReferenceTypeConverter<>));
        }

        private void CreateConverterFactory(Type type)
        {
            var converterFactoryType = typeof (ConverterFactory<>).MakeGenericType(type);

            _converterFactory = (IConverterFactory)Activator.CreateInstance(converterFactoryType);
        }
    }
}

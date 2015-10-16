using FluentAssertions;
using NUnit.Framework;
using SimpleAmazonSQS.Converters;

namespace SimpleAmazonSQS.Tests.Converters
{
    [TestFixture]
    public class ReferenceTypeConverterTests
    {
        private IConverter _converter;
        private FakePerson _person;
        private const string PersonJson = "{\"FamilyName\":\"Gandini\",\"Birthname\":\"Dexter\",\"PersonGender\":1}";

        [SetUp]
        public void SetUp()
        {
            _converter = new ReferenceTypeConverter<FakePerson>();
            _person = new FakePerson()
            {
                Birthname = "Dexter",
                FamilyName = "Gandini",
                PersonGender = FakePerson.Gender.Male
            };
        }

        [Test]
        public void ConvertToString_GivenReferenceType_ShouldConvert()
        {
            _converter
                .ConvertToString(_person)
                .Should()
                .Be(PersonJson);
        }

        [Test]
        public void ConvertFromString_GivenReferenceType_ShouldConvert()
        {
            _converter
                .ConvertFromString(PersonJson)
                .Should()
                .Be(_person);
        }
    }
}

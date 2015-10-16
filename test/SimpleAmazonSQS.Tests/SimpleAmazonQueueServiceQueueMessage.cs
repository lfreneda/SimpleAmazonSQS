using NUnit.Framework;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueServiceWithReferenceType : SimpleAmazonQueueServiceTypeBaseTests<FakePerson>
    {
        protected override FakePerson GetEnqueueItem()
        {
            return new FakePerson
            {
                FamilyName = "Gandini",
                Birthname = "Dexter",
                PersonGender = FakePerson.Gender.Female  
            };
        }
    }
}

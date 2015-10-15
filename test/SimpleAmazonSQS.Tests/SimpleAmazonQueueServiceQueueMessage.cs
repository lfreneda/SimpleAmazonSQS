using NUnit.Framework;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueServiceQueueMessage : SimpleAmazonQueueServiceTypeBaseTests<QueueMessage<FakeBody>>
    {
        protected override QueueMessage<FakeBody> GetEnqueueItem()
        {
            return new QueueMessage<FakeBody>(new FakeBody
            {
                FamilyName = "Gandini",
                Birthname = "Dexter"
            });
        }
    }
}

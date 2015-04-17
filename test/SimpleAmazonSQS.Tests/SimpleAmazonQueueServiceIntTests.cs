using NUnit.Framework;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueServiceIntTests : SimpleAmazonQueueServiceTypeBaseTests<int>
    {
        protected override int GetEnqueueItem()
        {
            return 10;
        }
    }
}
using NUnit.Framework;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueServiceLongTests : SimpleAmazonQueueServiceTypeBaseTests<long>
    {
        protected override long GetEnqueueItem()
        {
            return 123456789123456789;
        }
    }
}
using NUnit.Framework;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueServiceDecimalTests : SimpleAmazonQueueServiceTypeBaseTests<decimal>
    {
        protected override decimal GetEnqueueItem()
        {
            return 10.2M;
        }
    }
}
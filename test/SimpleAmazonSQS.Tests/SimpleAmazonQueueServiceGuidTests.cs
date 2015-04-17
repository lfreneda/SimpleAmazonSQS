using System;
using NUnit.Framework;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueServiceGuidTests : SimpleAmazonQueueServiceTypeBaseTests<Guid>
    {
        protected override Guid GetEnqueueItem()
        {
            return new Guid("9e3098ce-47c0-4610-9dbc-802a64ebd757");
        }
    }
}
using System;
using System.Collections.Generic;

namespace SimpleAmazonSQS
{
    public interface ISimpleAmazonQueueService
    {
        void Enqueue(Guid id);
        void DeleteMessage(string receiptHandle);
        IEnumerable<Guid> Dequeue(int messageCount);
        int Count();
    }
}
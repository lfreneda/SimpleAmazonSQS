using System;
using System.Collections.Generic;

namespace SimpleAmazonSQS
{
    public interface ISimpleAmazonQueueService<T> : IDisposable
    {
        void Enqueue(T id);
        void DeleteMessage(string receiptHandle);
        IEnumerable<T> Dequeue(int messageCount);
        int Count();
    }
}
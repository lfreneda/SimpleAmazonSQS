using System;

namespace SimpleAmazonSQS
{
    public struct QueueMessage<TBody> : IQueueMessage
    {
        private readonly TBody _body;

        public QueueMessage(TBody body)
        {
            _body = body;
        }

        public TBody Body
        {
            get { return _body; }
        }

        object IQueueMessage.Body
        {
            get { return _body; }
        }
    }
}
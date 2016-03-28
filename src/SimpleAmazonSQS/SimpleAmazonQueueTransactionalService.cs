using System.Collections.Generic;
using Amazon.SQS;
using SimpleAmazonSQS.Configuration;
using SimpleAmazonSQS.Converters;
using SimpleAmazonSQS.Entities;

namespace SimpleAmazonSQS
{
    public class SimpleAmazonQueueTransactionalService<T> : SimpleAmazonQueueService<T>
    {
        public SimpleAmazonQueueTransactionalService(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected SimpleAmazonQueueTransactionalService()
        {
        }

        internal SimpleAmazonQueueTransactionalService(IConfiguration configuration, IAmazonSQS amazonSqsClient, IConverterFactory converterFactory)
            : base(configuration, amazonSqsClient, converterFactory)
        {
        }

        public new virtual IEnumerable<DequeueResponse<T>> Dequeue(int messageCount)
        {
            var response = GetReceiveMessage(messageCount);

            if (response == null || response.Messages == null)
            {
                yield break;
            }

            foreach (var message in response.Messages)
            {
                var value = ConvertValue(message);

                if (value != null)
                {
                    yield return new DequeueResponse<T>(message.ReceiptHandle, (T)value);
                }
            }
        }
    }
}
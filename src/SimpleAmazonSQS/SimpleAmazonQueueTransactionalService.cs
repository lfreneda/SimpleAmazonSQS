using Amazon.SQS;
using SimpleAmazonSQS.Configuration;
using SimpleAmazonSQS.Converters;
using SimpleAmazonSQS.Entities;

namespace SimpleAmazonSQS
{
    public class SimpleAmazonQueueTransactionalService<T> : SimpleAmazonQueueService<DequeueResponse<T>>
    {
        public SimpleAmazonQueueTransactionalService(IConfiguration configuration)
            : base(configuration)
        {
        }

        internal SimpleAmazonQueueTransactionalService(IConfiguration configuration, IAmazonSQS amazonSqsClient, IConverterFactory converterFactory)
            : base(configuration, amazonSqsClient, converterFactory)
        {
        }

        protected override DequeueResponse<T> GetResponse(object value, string receiptHandle)
        {
            return new DequeueResponse<T>(receiptHandle, (T)value);
        }

        public void Complete(DequeueResponse<T> dequeueResponse)
        {
            DeleteMessage(dequeueResponse.ReceiptHandle);
        }
    }
}
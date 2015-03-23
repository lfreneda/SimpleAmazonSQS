using System;
using Amazon;
using Amazon.SQS;
using System.Linq;
using Amazon.SQS.Model;
using SimpleAmazonSQS.Exception;
using System.Collections.Generic;
using SimpleAmazonSQS.Configuration;

namespace SimpleAmazonSQS
{
    public class SimpleAmazonQueueService : ISimpleAmazonQueueService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonSQS _amazonSqsClient;
        private bool? _queueExists = null;

        protected SimpleAmazonQueueService()
        {
        }

        internal SimpleAmazonQueueService(IConfiguration configuration, IAmazonSQS amazonSqsClient)
        {
            _configuration = configuration;
            _amazonSqsClient = amazonSqsClient;
        }

        public SimpleAmazonQueueService(IConfiguration configuration)
            : this(configuration, AWSClientFactory.CreateAmazonSQSClient(configuration.AccessKey, configuration.SecretKey, new AmazonSQSConfig { ServiceURL = configuration.ServiceUrl }))
        {

        }

        public virtual bool QueueExists()
        {
            if (_queueExists.HasValue) return _queueExists.Value;

            _queueExists = false;

            var queues = _amazonSqsClient.ListQueues(new ListQueuesRequest());
            if (queues != null)
            {
                _queueExists = queues.QueueUrls.Any(queue => queue == _configuration.QueueUrl);
            }

            return _queueExists.Value;
        }

        public virtual void Enqueue(Guid id)
        {
            if (!QueueExists())
            {
                throw new SimpleAmazonSqsException("Queue is not available or could not be created.");
            }

            _amazonSqsClient.SendMessage(new SendMessageRequest
            {
                QueueUrl = _configuration.QueueUrl,
                MessageBody = id.ToString()
            });
        }

        public virtual void DeleteMessage(string receiptHandle)
        {
            _amazonSqsClient.DeleteMessage(new DeleteMessageRequest
            {
                ReceiptHandle = receiptHandle,
                QueueUrl = _configuration.QueueUrl
            });
        }

        public IEnumerable<Guid> Dequeue(int messageCount = 1)
        {
            if (messageCount < 1 || messageCount > 10)
            {
                throw new ArgumentOutOfRangeException("messageCount", "messageCount must be between 1 and 10.");
            }

            var response = _amazonSqsClient.ReceiveMessage(new ReceiveMessageRequest
            {
                QueueUrl = _configuration.QueueUrl,
                MaxNumberOfMessages = messageCount
            });

            if (response != null && response.Messages.Any())
            {
                foreach (var message in response.Messages)
                {
                    Guid guid;
                    if (Guid.TryParse(message.Body, out guid))
                    {
                        yield return guid;
                    }

                    DeleteMessage(message.ReceiptHandle);
                }
            }
        }

        public int Count()
        {
            var response = _amazonSqsClient.GetQueueAttributes(new GetQueueAttributesRequest
            {
                QueueUrl = _configuration.QueueUrl,
                AttributeNames = new List<string>(new[] { "ApproximateNumberOfMessages" })
            });

            if (response == null) return 0;
            return response.ApproximateNumberOfMessages;
        }
    }
}

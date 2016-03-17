using System;
using Amazon.SQS;
using System.Linq;
using Amazon.SQS.Model;
using SimpleAmazonSQS.Exception;
using System.Collections.Generic;
using Amazon.Runtime;
using SimpleAmazonSQS.Configuration;
using SimpleAmazonSQS.Converters;

namespace SimpleAmazonSQS
{
    public class SimpleAmazonQueueService<T> : ISimpleAmazonQueueService<T>, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonSQS _amazonSqsClient;
        private readonly IConverter _converter;
        private bool? _queueExists = null;

        private const int DefaultVisibilityTimeout = 30;

        protected SimpleAmazonQueueService()
        {
        }

        internal SimpleAmazonQueueService(IConfiguration configuration, IAmazonSQS amazonSqsClient, IConverterFactory converterFactory)
            : this()
        {
            _configuration = configuration;
            _amazonSqsClient = amazonSqsClient;
            _converter = converterFactory.Create();
        }

        public SimpleAmazonQueueService(IConfiguration configuration)
            : this(configuration, 
                  new AmazonSQSClient(new BasicAWSCredentials(configuration.AccessKey, configuration.SecretKey), new AmazonSQSConfig { ServiceURL = configuration.ServiceUrl }),
                  new ConverterFactory<T>())
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

        public virtual void Enqueue(T id)
        {
            if (!QueueExists())
            {
                throw new SimpleAmazonSqsException("Queue is not available or could not be created.");
            }

            _amazonSqsClient.SendMessage(new SendMessageRequest
            {
                QueueUrl = _configuration.QueueUrl,
                MessageBody = _converter.ConvertToString(id)
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

        public IEnumerable<T> Dequeue(int messageCount = 1)
        {
            if (messageCount < 1 || messageCount > 10)
            {
                throw new ArgumentOutOfRangeException("messageCount", "messageCount must be between 1 and 10.");
            }
            
            var response = _amazonSqsClient.ReceiveMessage(new ReceiveMessageRequest
            {
                QueueUrl = _configuration.QueueUrl,
                MaxNumberOfMessages = messageCount,
                VisibilityTimeout = _configuration.VisibilityTimeout ?? DefaultVisibilityTimeout
            });

            if (response == null || response.Messages == null)
            {
                yield break;
            }

            foreach (var message in response.Messages)
            {
                var value = ConvertValue(message);

                if (value != null)
                {
                    yield return GetResponse(value, message.ReceiptHandle);
                }
                else
                {
                    DeleteMessage(message.ReceiptHandle);
                }
            }
        }

        protected virtual T GetResponse(object value, string receiptHandle)
        {
            DeleteMessage(receiptHandle);
            return (T)value;
        }

        private object ConvertValue(Message message)
        {
            try
            {
                return _converter.ConvertFromString(message.Body);
            }
            catch
            {
                return null;
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

        public void Dispose()
        {
            if (_amazonSqsClient != null)
            {
                _amazonSqsClient.Dispose();
            }
        }
    }
}
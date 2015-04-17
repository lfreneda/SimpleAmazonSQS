using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SimpleAmazonSQS.Configuration;

namespace SimpleAmazonSQS.Tests
{
    public abstract class SimpleAmazonQueueServiceTypeBaseTests<T> where T : struct
    {
        private SimpleAmazonQueueService<T> _simpleAmazonQueueService;
        private Mock<IAmazonSQS> _fakeAmazonSqs;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [SetUp]
        public void SetUp()
        {
            _fakeAmazonSqs = new Mock<IAmazonSQS>();
            _simpleAmazonQueueService = new SimpleAmazonQueueService<T>(
                configuration: new CustomConfiguration { SecretKey = "FakeSecretKey", AccessKey = "FakeAccessKey", QueueUrl = "http://queueurl.aws.com" },
                amazonSqsClient: _fakeAmazonSqs.Object
                );
        }

        protected abstract T GetEnqueueItem();

        private Message CreateMessageWithCorrectFormatString()
        {
            return new Message
            {
                Body = GetEnqueueItem().ToString()
            };
        }

        [Test]
        public void Enqueue_GivenAGuid_ShouldCallAmazonClientWithExpectedData()
        {
            _fakeAmazonSqs.Setup(c => c.ListQueues(It.IsAny<ListQueuesRequest>()))
                .Returns(new ListQueuesResponse
                {
                    QueueUrls = new List<string>
                    {
                        "http://queueurl.aws.com"
                    }
                });

            var id = GetEnqueueItem();
            var idAsString = id.ToString();

            _simpleAmazonQueueService.Enqueue(id);

            _fakeAmazonSqs.Verify(mock =>
                mock.SendMessage(It.Is<SendMessageRequest>(parameters => parameters.QueueUrl == "http://queueurl.aws.com" && parameters.MessageBody == idAsString)
                    ), Times.Once);
        }

        [Test]
        public void Dequeue_GivenMessagesReturnedFromAamazonSqs_ShouldBeParsedToGuidList()
        {
            _fakeAmazonSqs.Setup(c => c.ReceiveMessage(It.IsAny<ReceiveMessageRequest>()))
                .Returns(new ReceiveMessageResponse
                {
                    Messages = new List<Message>
                    {
                        CreateMessageWithCorrectFormatString(),
                        CreateMessageWithCorrectFormatString(),
                        CreateMessageWithCorrectFormatString(),
                    }
                });

            var messages = _simpleAmazonQueueService.Dequeue(messageCount: 3);

            messages.ToList().Should().BeEquivalentTo(new[]
            {
                GetEnqueueItem(),
                GetEnqueueItem(),
                GetEnqueueItem()
            });
        }

        [Test]
        public void Dequeue_GivenMessageOnQueueThatIsNotAGuid_ShouldBeIgnored()
        {
            _fakeAmazonSqs.Setup(c => c.ReceiveMessage(It.IsAny<ReceiveMessageRequest>()))
                .Returns(new ReceiveMessageResponse
                {
                    Messages = new List<Message>
                    {
                        CreateMessageWithCorrectFormatString(),
                new Message() { Body = "i'm invalid message" },
                        CreateMessageWithCorrectFormatString(),
                    }
                });

            var messages = _simpleAmazonQueueService.Dequeue(messageCount: 3);

            messages.ToList().Should().BeEquivalentTo(new[]
            {
                GetEnqueueItem(),
                GetEnqueueItem(),
            });
        }

        [Test]
        public void Dequeue_WhenDequeing_DeleteMessageShouldBeCalledForEachMessage()
        {
            _fakeAmazonSqs.Setup(c => c.ReceiveMessage(It.IsAny<ReceiveMessageRequest>()))
                .Returns(new ReceiveMessageResponse
                {
                    Messages = new List<Message>
                    {
                        CreateMessageWithCorrectFormatString(),
                      new Message() { Body = "i'm invalid message" },
                        CreateMessageWithCorrectFormatString(),
                    }
                });

            var messages = _simpleAmazonQueueService.Dequeue(messageCount: 3).ToList();

            _fakeAmazonSqs.Verify(c => c.DeleteMessage(It.IsAny<DeleteMessageRequest>()), Times.Exactly(3));
        }
    }
}
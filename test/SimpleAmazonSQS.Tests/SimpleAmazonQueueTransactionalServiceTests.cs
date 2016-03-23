using System;
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
using SimpleAmazonSQS.Converters;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueTransactionalServiceTests
    {
        private SimpleAmazonQueueTransactionalService<int> _simpleAmazonQueueService;

        private Mock<IAmazonSQS> _fakeAmazonSqs;
        private Mock<IConfiguration> _fakeConfiguration;

        private const string ReceiptHandle = "abc123";

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

            _fakeAmazonSqs
                .Setup(e => e.ReceiveMessage(It.IsAny<ReceiveMessageRequest>()))
                .Returns(new ReceiveMessageResponse
                {
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            ReceiptHandle = ReceiptHandle,
                            Body = "123"
                        }
                    }
                });

            _fakeConfiguration = new Mock<IConfiguration>();
            _fakeConfiguration.SetupGet(e => e.QueueUrl).Returns("http://queueurl.aws.com");

            _simpleAmazonQueueService = new SimpleAmazonQueueTransactionalService<int>(
                configuration: _fakeConfiguration.Object,
                amazonSqsClient: _fakeAmazonSqs.Object,
                converterFactory: new ConverterFactory<int>());
        }

        [Test]
        public void Dequeue_GivenMessageCountGreaterThan10_ShouldThrowExpectedException()
        {
            Action action = () =>
            {
                var messages = _simpleAmazonQueueService.Dequeue(messageCount: 11).ToList();
            };

            action
                .ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage("messageCount must be between 1 and 10.\r\nParameter name: messageCount");
        }

        [Test]
        public void Dequeue_GivenMessageCount_ShouldNotDeleteMessage()
        {
            var messages = _simpleAmazonQueueService.Dequeue(messageCount: 10).ToList();

            _fakeAmazonSqs.Verify(e => e.DeleteMessage(It.IsAny<DeleteMessageRequest>()), Times.Never);
        }

        [Test]
        public void Dequeue_GivenMessageCount_ShouldReturnExpectedValue()
        {
            var message = _simpleAmazonQueueService.Dequeue(messageCount: 1).ToList().First();

            Assert.AreEqual(123, message.Value);
        }

        [Test]
        public void Dequeue_GivenMessageCount_ShouldReturnExpectedReceiptHandle()
        {
            var message = _simpleAmazonQueueService.Dequeue(messageCount: 1).ToList().First();

            Assert.AreEqual(ReceiptHandle, message.ReceiptHandle);
        }

        [Test]
        public void Dequeue_WhenReceiveNullReceiveMessageResponse_ShouldReturnEmptyList()
        {
            _fakeAmazonSqs
                .Setup(e => e.ReceiveMessage(It.IsAny<ReceiveMessageRequest>()))
                .Returns((ReceiveMessageResponse)null);

            var messages = _simpleAmazonQueueService.Dequeue(messageCount: 1).ToList();

            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public void Dequeue_WhenReceiveNullResponseMessages_ShouldReturnEmptyList()
        {
            _fakeAmazonSqs
                .Setup(e => e.ReceiveMessage(It.IsAny<ReceiveMessageRequest>()))
                .Returns(new ReceiveMessageResponse
                {
                    Messages = null
                });

            var messages = _simpleAmazonQueueService.Dequeue(messageCount: 1).ToList();

            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public void DeleteMessage_GivenReceiptHandle_ShouldDeleteMessage()
        {
            _simpleAmazonQueueService.DeleteMessage(ReceiptHandle);

            _fakeAmazonSqs.Verify(e => e.DeleteMessage(It.IsAny<DeleteMessageRequest>()), Times.Once());
        }

        [Test]
        public void Dispose_WhenDisposingSimpleAmazonQueueTransactionService_ShouldDisposeAmazonSqsClient()
        {
            _simpleAmazonQueueService.Dispose();

            _fakeAmazonSqs.Verify(e => e.Dispose(), Times.Once);
        }
    }
}
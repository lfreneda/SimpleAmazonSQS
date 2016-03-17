using Amazon.SQS;
using Amazon.SQS.Model;
using Moq;
using NUnit.Framework;
using SimpleAmazonSQS.Configuration;
using SimpleAmazonSQS.Converters;
using SimpleAmazonSQS.Entities;

namespace SimpleAmazonSQS.Tests
{
    [TestFixture]
    public class SimpleAmazonQueueTransactionalServiceTests
    {
        private SimpleAmazonQueueTransactionalService<int> _simpleAmazonQueueService;

        private Mock<IAmazonSQS> _fakeAmazonSqs;
        private Mock<IConfiguration> _fakeConfiguration;

        private DequeueResponse<int> _dequeueResponse;
        private const string ReceiptHandle = "abc123";

        [SetUp]
        public void SetUp()
        {
            _fakeAmazonSqs = new Mock<IAmazonSQS>();

            _fakeConfiguration = new Mock<IConfiguration>();
            _fakeConfiguration.SetupGet(e => e.QueueUrl).Returns("http://queueurl.aws.com");

            _simpleAmazonQueueService = new SimpleAmazonQueueTransactionalService<int>(
                configuration: _fakeConfiguration.Object,
                amazonSqsClient: _fakeAmazonSqs.Object,
                converterFactory: new ConverterFactory<int>());

            _dequeueResponse = new DequeueResponse<int>(ReceiptHandle, 0);
        }

        [Test]
        public void Complete_GivenDequeueResponse_ShouldCallDeleteMessageWithExpectedReceiptHandle()
        {
            DeleteMessageRequest deleteMessageRequestCallback = null;

            _fakeAmazonSqs
                .Setup(e => e.DeleteMessage(It.IsAny<DeleteMessageRequest>()))
                .Callback<DeleteMessageRequest>(callback => deleteMessageRequestCallback = callback);

            _simpleAmazonQueueService.Complete(_dequeueResponse);

            Assert.AreEqual(ReceiptHandle, deleteMessageRequestCallback.ReceiptHandle);
        }
    }
}
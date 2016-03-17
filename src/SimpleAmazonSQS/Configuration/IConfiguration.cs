namespace SimpleAmazonSQS.Configuration
{
    public interface IConfiguration
    {
        string AccessKey { get; }
        string SecretKey { get; }
        string QueueUrl { get; }
        string ServiceUrl { get; }
        int? VisibilityTimeout { get; }
    }
}
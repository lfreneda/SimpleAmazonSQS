using System;
using System.Configuration;

namespace SimpleAmazonSQS.Configuration
{
    public class AppSettingsConfiguration : IConfiguration
    {
        public string AccessKey { get { return ConfigurationManager.AppSettings["simpleAmazonSQS.accessKey"]; } }

        public string SecretKey { get { return ConfigurationManager.AppSettings["simpleAmazonSQS.secretKey"]; } }

        public string QueueUrl { get { return ConfigurationManager.AppSettings["simpleAmazonSQS.queueUrl"]; } }

        public string ServiceUrl { get { return ConfigurationManager.AppSettings["simpleAmazonSQS.serviceUrl"]; } }

        public int? VisibilityTimeout { get { return ConfigurationManager.AppSettings["simpleAmazonSQS.visibilityTimeout"] != null ? 
                    Convert.ToInt32(ConfigurationManager.AppSettings["simpleAmazonSQS.visibilityTimeout"]) 
                    : (int?)null; } }
    }
}
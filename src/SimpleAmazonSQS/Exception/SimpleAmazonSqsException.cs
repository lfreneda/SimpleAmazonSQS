using System;

namespace SimpleAmazonSQS.Exception
{
    public class SimpleAmazonSqsException : ApplicationException
    {
        public SimpleAmazonSqsException(string message)
            : base(message)
        {
        }
    }
}
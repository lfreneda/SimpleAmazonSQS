using System;
namespace SimpleAmazonSQS.Converters
{
    internal interface IConverter
    {
        string ConvertToString(object value);

        object ConvertFromString(string body);
    }
}
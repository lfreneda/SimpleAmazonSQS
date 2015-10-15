using System;
namespace SimpleAmazonSQS.Converters
{
    internal interface IConverter
    {
        string ConvertToString(ValueType valueType);

        object ConvertFromString(string body);
    }
}
using System;
using System.ComponentModel;

namespace SimpleAmazonSQS.Converters
{
    internal class DefaultConverter<T> : IConverter
        where T : struct
    {
        public string ConvertToString(ValueType valueType)
        {
            return valueType.ToString();
        }

        public object ConvertFromString(string body)
        {
            return TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(body);
        }
    }
}
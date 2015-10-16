using System;
using System.ComponentModel;

namespace SimpleAmazonSQS.Converters
{
    internal class ValueTypeConverter<T> : IConverter
    {
        private readonly TypeConverter _converter;

        public ValueTypeConverter()
        {
            var currentType = typeof (T);

            if (!currentType.IsValueType)
            {
                throw new InvalidOperationException(string.Format("The {0} type must be Value Type", currentType.Name));
            }

            _converter = TypeDescriptor.GetConverter(currentType);

        }

        public string ConvertToString(object value)
        {
            return _converter.ConvertToInvariantString(value);
        }

        public object ConvertFromString(string body)
        {
            return _converter.ConvertFromInvariantString(body);
        }
    }
}
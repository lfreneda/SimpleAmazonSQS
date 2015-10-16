using System;
using Newtonsoft.Json;
namespace SimpleAmazonSQS.Converters
{
    internal class ReferenceTypeConverter<T> : IConverter
    {
        private readonly Type _bodyType;

        internal ReferenceTypeConverter()
        {
            _bodyType = typeof(T);
        }
        
        public string ConvertToString(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public object ConvertFromString(string body)
        {
            return JsonConvert.DeserializeObject(body, _bodyType);
        }
    }
}
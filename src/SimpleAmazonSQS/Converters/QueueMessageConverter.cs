using System;
using System.Reflection;
using Newtonsoft.Json;
namespace SimpleAmazonSQS.Converters
{
    public class QueueMessageConverter : IConverter
    {
        private readonly Type _bodyType;

        public QueueMessageConverter(Type bodyType)
        {
            _bodyType = bodyType;
        }
        
        public string ConvertToString(ValueType valueType)
        {
            return JsonConvert.SerializeObject((valueType as IQueueMessage).Body);
        }

        public object ConvertFromString(string body)
        {
            var queueMessageType = typeof (QueueMessage<>).MakeGenericType(_bodyType);

            var bodyValue = JsonConvert.DeserializeObject(body, _bodyType);

            return Activator.CreateInstance(queueMessageType, bodyValue);
        }
    }
}
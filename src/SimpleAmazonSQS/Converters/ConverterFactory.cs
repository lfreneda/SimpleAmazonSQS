using System.Linq;
namespace SimpleAmazonSQS.Converters
{
    internal class ConverterFactory<T>
        where T : struct
    {
        public virtual IConverter Create()
        {
            var type = typeof (T);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (QueueMessage<>))
            {
                return new QueueMessageConverter(type.GetGenericArguments().First());
            }

            return new DefaultConverter<T>();
        }
    }
}
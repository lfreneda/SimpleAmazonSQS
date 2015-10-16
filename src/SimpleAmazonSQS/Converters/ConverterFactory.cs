namespace SimpleAmazonSQS.Converters
{
    internal class ConverterFactory<T> : IConverterFactory
    {
        public virtual IConverter Create()
        {
            var type = typeof (T);
            
            return
                type.IsValueType ?
                    new ValueTypeConverter<T>() as IConverter: 
                    new ReferenceTypeConverter<T>();
        }
    }
}
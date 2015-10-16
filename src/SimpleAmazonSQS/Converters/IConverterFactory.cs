namespace SimpleAmazonSQS.Converters
{
    internal interface IConverterFactory
    {
        IConverter Create();
    }
}
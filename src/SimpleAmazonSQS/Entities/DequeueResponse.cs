namespace SimpleAmazonSQS.Entities
{
    public class DequeueResponse<T>
    {
        private readonly string _receiptHandle;
        private readonly T _value;
        
        public DequeueResponse(string receiptHandle, T value)
        {
            _receiptHandle = receiptHandle;
            _value = value;
        }

        public T Value { get { return _value; } }

        public string ReceiptHandle { get { return _receiptHandle; } }
    }
}
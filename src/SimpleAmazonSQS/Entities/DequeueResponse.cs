namespace SimpleAmazonSQS.Entities
{
    public class DequeueResponse<T> 
    {
        internal readonly string ReceiptHandle;
        private readonly T _value;
        
        internal DequeueResponse(string receiptHandle, T value)
        {
            ReceiptHandle = receiptHandle;
            _value = value;
        }

        public T Value { get { return _value; } }
    }
}
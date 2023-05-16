namespace EventBus.Messages.Common
{
    public class ResponseMessage<T>
    {
        public ResponseMessage()
        {
        }
        public ResponseMessage(string errorMessage)
        {
            Message = errorMessage;
        }
        public ResponseMessage(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}

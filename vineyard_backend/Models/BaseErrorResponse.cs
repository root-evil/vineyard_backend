namespace vineyard_backend.Models
{
    public class BaseResponseError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string TraceId { get; set; }

        public BaseResponseError(string message = null, string description = null, int code = -1)
        {
            Message = message;
            Description = description ?? message;
            Code = code;
        }

        public BaseResponseError()
        {
            Code = -1;
        }
    }
}
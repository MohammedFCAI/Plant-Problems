namespace Plant_Problems.Core.Bases
{
    public class Response<T>
    {
        public bool Succeed { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public Response()
        {

        }

        public Response(T data, string message = null)
        {
            Succeed = true;
            Message = message;
        }

        public Response(string message, bool succeed)
        {
            Succeed = succeed;
            Message = message;
        }
    }
}

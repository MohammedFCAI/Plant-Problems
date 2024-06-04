namespace Plant_Problems.API
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
        public string ErrorMessages { get; set; }
    }
}

namespace fileManagement.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public ResultBody<T> result { get; set; }
        public ResponseError Errors { get; set; }
    }

    public class ResultBody<T>
    {
        public int totalAcount { get; set; }
        public IReadOnlyList<T> items { get; set; }
    }

    public class ResponseError
    {
        public IEnumerable<Error> Error { get; set; }

        public bool UnAuthorizedRequest { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }
    }
}

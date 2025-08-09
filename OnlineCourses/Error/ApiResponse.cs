namespace OnlineCourses.Error
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? ReportMessage { get; set; }
        public ApiResponse(int statuscode, string? message)
        {
            StatusCode = statuscode;
            ReportMessage = message ?? GetDefaultMessageForStutesCode(StatusCode);
        }

        private string? GetDefaultMessageForStutesCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request(You send a wrong request)",
                401 => "You are not Authorized",
                404 => "Resource Not Found",
                500 => "Internal server error",
                _ => null
            };
        }
    }
}

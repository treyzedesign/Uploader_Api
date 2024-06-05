namespace Uploader_Api.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Token { get; set; }

        public string ErrorMessage { get; set; }
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public ApiResponse()
        {
            Success = true;
            StatusCode = StatusCodes.Status200OK;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                StatusCode = StatusCodes.Status200OK,
                Message = message
            };
        }
        public static ApiResponse<T> LoginResponse(T data,string Token, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Token = Token,
                StatusCode = StatusCodes.Status200OK,
                Message = message
            };
        }

        public static ApiResponse<T> ErrorResponse(string errorMessage, int statusCode)
        {
            return new ApiResponse<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}

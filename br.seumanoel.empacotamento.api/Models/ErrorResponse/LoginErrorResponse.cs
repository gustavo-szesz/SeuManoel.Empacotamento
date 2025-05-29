namespace br.seumanoel.empacotamento.api.Models.ErrorResponse
{
    public class LoginErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        /// <example>The credentials are invalid, 401 Unauthorized</example>
        public string Message { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        /// <example>INVALID_CREDENTIALS</example>
        public string ErrorCode { get; set; }

        /// <summary>
        /// New error response
        /// </summary>
        public LoginErrorResponse(string message, string errorCode = null)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
    
}
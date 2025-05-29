namespace br.seumanoel.empacotamento.api.Models.ErrorResponse
{
    /// <summary>
    /// Standardized error response format
    /// </summary>
    public class UserErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        /// <example>Username `newl2devjr` alredy in exists</example>
        public string Message { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        /// <example>USER_EXISTS</example>
        public string ErrorCode { get; set; }

        /// <summary>
        /// New error response
        /// </summary>
        public UserErrorResponse(string message, string errorCode = null)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
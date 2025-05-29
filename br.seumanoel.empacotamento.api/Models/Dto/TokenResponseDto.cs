namespace br.seumanoel.empacotamento.api.Models.Dto
{
    /// <summary>
    /// Authentication token response
    /// </summary>
        public class TokenResponseDto
        {
            /// <summary>
            /// JWT authentication token
            /// </summary>
            /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
            public string Token { get; set; }
        }
    
}
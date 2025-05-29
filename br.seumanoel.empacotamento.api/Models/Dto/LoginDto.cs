using System.ComponentModel.DataAnnotations;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    /// <summary>
    /// Login credentials
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Username for authentication
        /// </summary>
        /// <example>newl2devjr</example>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <example>kashmir</example>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
    
}

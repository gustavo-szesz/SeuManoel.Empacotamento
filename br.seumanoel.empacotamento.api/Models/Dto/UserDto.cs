using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    /// <summary>
    /// Data Transfer Object for User
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Username 
        /// </summary>
        /// <example>newl2devjr</example>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        /// <summary>
        /// Password 
        /// </summary>
        /// <example>kashmir</example>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
    }

    public class UserResponseDto
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
    }


}


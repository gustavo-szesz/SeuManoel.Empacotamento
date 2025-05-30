using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Models.ErrorResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace br.seumanoel.empacotamento.api.Controllers
{
    /// <summary>
    /// Controller for handling user account creation.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Tags("01-Create Account")]
    public class UserController : ControllerBase
    {
        #region Constructor
        /// <summary>
        /// Initialize DbContext
        /// </summary>
        private readonly AppDbContext _context;

        
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        #endregion
        

        #region Endpoint Create User
        /// <summary>
        /// Create new user account.
        /// </summary>
        /// <param name="userDto">User data for creating a new account</param>
        /// <returns>The created user's ID and username</returns>
        /// <response code="201">Returns the created user ID and username.</response>
        /// <response code="400">Returns BadRequest if user already exists.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CreationUserErrorResponse))]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            // Validate if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
                return BadRequest(new CreationUserErrorResponse("Usuário já existe."));

            // Map DTO to entity
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password
            };

            // Hash SHA256 
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            user.Password = Convert.ToBase64String(hash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return response DTO
            var responseDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username
            };

            return CreatedAtAction(nameof(CreateUser),
                                  new { id = user.Id },
                                  responseDto);
        }
        #endregion
    }
}

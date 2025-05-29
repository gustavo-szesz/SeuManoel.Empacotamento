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
        /// <summary>
        /// Initialize DbContext
        /// </summary>
        private readonly AppDbContext _context;

        #region Constructor
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Endpoint Create User
        /// <summary>
        /// Create new user account.
        /// </summary>
        /// <param name="user"></param>
        /// <returns> Return only user ID and username </returns>
        /// <response code="201">Returns the created user ID and username.</response>
        /// <response code="400">Return BadRequest if user already exists.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = (typeof(UserDto)))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = (typeof(UserErrorResponse)))]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest("Usuário já existe.");

            // Hash SHA256 
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            user.Password = Convert.ToBase64String(hash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser),
                                    new { id = user.Id },
                                    new { user.Id, user.Username });
        }
        #endregion
    }
}

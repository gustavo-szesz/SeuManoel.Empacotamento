using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace br.seumanoel.empacotamento.api.Controllers
{
    /// <summary>
    /// Controller for handling user login and JWT token generation.
    /// 
    [ApiController]
    [Route("[controller]")]
    [Tags("02-Authentication")]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Inicilize instance AuthService
        /// </summary>
        private readonly AuthService _authService;

        #region Constructor
        public LoginController(AuthService authService)
        {
            _authService = authService;
        }
        #endregion


        #region Endpoint Login
        /// <summary>
        /// Autenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="login">Login credential</param>
        /// <returns> Return JWT for authenticated user</returns>
        /// <response code="200">Returns the JWT token if authentication is ok.</response>
        /// <response code="401">Return Unauthorized </response>
        [HttpPost("account")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(TokenResponseDto)))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username and password are required.");
            }
            // Only example
            if (login.Username == "l2devjr" && login.Password == "senha123")
            {
                return Ok(new { token = GenerateJwtToken(login.Username) });
            }

            // Validação real
            var user = await _authService.AuthenticateAsync(login.Username, login.Password);
            if (user != null)
            {
                return Ok(new { token = GenerateJwtToken(user.Username) });
            }

            return Unauthorized();
        }
        #endregion


        #region  JWT Token Generation
        /// <summary>
        /// Generete JWT
        /// </summary>
        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER"),
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}

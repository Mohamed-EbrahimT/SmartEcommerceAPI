using Microsoft.AspNetCore.Mvc;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS.Auth;

namespace SmartE_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="dto">User registration details</param>
        /// <returns>JWT token and user information on success</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(dto);

            if (result == null)
            {
                return BadRequest(new { message = "Email already registered" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Login with existing credentials
        /// </summary>
        /// <param name="dto">Login credentials (email and password)</param>
        /// <returns>JWT token and user information on success</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }
    }
}

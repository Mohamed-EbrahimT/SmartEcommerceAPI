using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinalProj.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS.Auth;
using SmartE_Commerce_Data.Contracts;

namespace SmartE_Commerce_Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, ICartRepository cartRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterDTO dto)
        {
            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                return null; // Email already registered
            }

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Calculate age if date of birth is provided
            int? age = null;
            if (dto.DateOfBirth.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                age = today.Year - dto.DateOfBirth.Value.Year;
                if (dto.DateOfBirth.Value > today.AddYears(-age.Value))
                    age--;
            }

            // Create new user with Customer role (UserRoleId = 2)
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                Age = age,
                UserRoleId = 2 // Customer role
            };

            var createdUser = await _userRepository.InsertAsync(user);

            // Create cart for new user
            await EnsureCartExistsAsync(createdUser.UserId);

            // Get user with role for token generation
            var userWithRole = await _userRepository.GetByEmailAsync(createdUser.Email);

            return GenerateAuthResponse(userWithRole!);
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
        {
            // Get user by email
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return null; // User not found
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return null; // Invalid password
            }

            // Ensure user has a cart (for users created before this feature)
            await EnsureCartExistsAsync(user.UserId);

            return GenerateAuthResponse(user);
        }

        private async Task EnsureCartExistsAsync(int userId)
        {
            var existingCart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (existingCart == null)
            {
                var cart = new Cart
                {
                    UserId = userId,
                    Total = 0,
                    ItemTotal = 0
                };
                await _cartRepository.InsertAsync(cart);
            }
        }

        private AuthResponseDTO GenerateAuthResponse(User user)
        {
            var expirationMinutes = int.Parse(_configuration["JWT:ExpirationInMinutes"] ?? "60");
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = GenerateJwtToken(user, expiresAt);

            return new AuthResponseDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = user.UserRole?.UserRoleName ?? "Customer",
                Token = token,
                ExpiresAt = expiresAt
            };
        }

        private string GenerateJwtToken(User user, DateTime expiresAt)
        {
            var secretKey = _configuration["JWT:SecretKey"];
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.UserRole?.UserRoleName ?? "Customer"),
                new Claim("userId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

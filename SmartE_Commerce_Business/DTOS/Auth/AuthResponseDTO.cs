namespace SmartE_Commerce_Business.DTOS.Auth
{
    public class AuthResponseDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}

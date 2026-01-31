using SmartE_Commerce_Business.DTOS.Auth;

namespace SmartE_Commerce_Business.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);
    }
}

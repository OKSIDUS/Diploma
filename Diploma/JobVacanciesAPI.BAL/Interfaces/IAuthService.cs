using JobVacanciesAPI.BAL.DTOs.Auth;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginDTO login);
        Task<AuthResponseDTO?> RegisterAsync(RegisterDTO register);
    }
}

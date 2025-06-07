using JobVacanciesAPP.BAL.DTOs.Auth;

namespace JobVacanciesAPP.BAL.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO> Login(LoginDTO loginDTO);
        Task<ResponseDTO> Register(RegisterDTO registerDTO);
    }
}

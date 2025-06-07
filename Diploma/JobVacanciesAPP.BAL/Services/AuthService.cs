using JobVacanciesAPP.BAL.DTOs.Auth;
using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.DAL.Interfaces;

namespace JobVacanciesAPP.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }


        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {
            if (loginDTO != null)
            {
               var result = await _authRepository.LoginAsync(new DAL.Models.Auth.Login { Email = loginDTO.Email, Password = loginDTO.Password });
               if (result != null)
                {
                    return new ResponseDTO
                    {
                        Role = result.Role,
                        Token = result.Token,
                        UserId = result.UserId,
                    };
                }
            }

            return null;
        }

        public async Task<ResponseDTO> Register(RegisterDTO registerDTO)
        {
            if (registerDTO != null)
            {
                var result = await _authRepository.RegisterAsync(new DAL.Models.Auth.Register { Email = registerDTO.Email, Password = registerDTO.Password, Role = registerDTO.Role });
                if (result != null)
                {
                    return new ResponseDTO
                    {
                        Role = result.Role,
                        Token = result.Token,
                        UserId = result.UserId,
                    };
                }
            }

            return null;
        }
    }
}

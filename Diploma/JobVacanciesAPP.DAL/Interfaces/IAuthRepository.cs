using JobVacanciesAPP.DAL.Models.Auth;

namespace JobVacanciesAPP.DAL.Interfaces
{
    public interface IAuthRepository
    {
        Task<Response> LoginAsync(Login login);
        Task<Response> RegisterAsync(Register register);
    }
}

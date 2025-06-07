using JobVacanciesAPP.DAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Auth;
using System.Net.Http.Json;
using System.Text.Json;

namespace JobVacanciesAPP.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApiSettings api;
        private HttpClient httpClient = new HttpClient();
        public AuthRepository(ApiSettings apiSetting)
        {
            api = apiSetting;
            httpClient.BaseAddress = new Uri(api.BaseUrl);
        }
        public async Task<Response> LoginAsync(Login login)
        {
            if (login != null)
            {
                var json = JsonSerializer.Serialize(login);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress + "auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<Response>();
                    return authResponse;
                }
            }
            return null;
        }

        public async Task<Response> RegisterAsync(Register register)
        {
            if (register != null)
            {
                var json = JsonSerializer.Serialize(register);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress + "auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<Response>();
                    return authResponse;
                }
            }
            return null;
        }
    }
}

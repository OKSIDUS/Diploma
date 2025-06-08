using JobVacanciesAPP.DAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Auth;
using JobVacanciesAPP.DAL.Models.Users;
using JobVacanciesAPP.DAL.Models.Vacancy;
using System.Net.Http.Json;
using System.Text.Json;

namespace JobVacanciesAPP.DAL.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ApiSettings api;
        private HttpClient httpClient = new HttpClient();
        public VacancyRepository(ApiSettings apiSetting)
        {
            api = apiSetting;
            httpClient.BaseAddress = new Uri(api.BaseUrl);
        }

        public async Task CreateVacancy(CreateVacancy vacancy)
        {
            if (vacancy != null)
            {
                var json = JsonSerializer.Serialize(vacancy);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress + "vacancy/create-vacancy", content);

                if (response.IsSuccessStatusCode)
                {
                }
            }
        }

        public async Task<List<string>> GetAllSkills()
        {
            var response = await httpClient.GetAsync($"tag/get-all");

            if (response.IsSuccessStatusCode)
            {
                var skills = await response.Content.ReadFromJsonAsync<List<string>>();
                return skills;
            }

            return null;
        }

        public async Task<VacancyPage> GetRecommendetVacancies(int page, int pageSize, bool isRecommendation, int userId)
        {
            var response = await httpClient.GetAsync($"vacancy/get-vacancies-page?page={page}&pageSize={pageSize}&isRecommendation={isRecommendation.ToString().ToLower()}&userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyPage>();
                return vacancyPage;
            }

            return null;
        }

        public async Task<VacancyPage> GetVacanciesForRecruiter(int page, int pageSize, bool isRecommendation, int userId)
        {
            var response = await httpClient.GetAsync($"vacancy/get-vacancies-page?page={page}&pageSize={pageSize}&isRecommendation={isRecommendation.ToString().ToLower()}&userId={userId}");

            if(response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyPage>();
                return vacancyPage;
            }

            return null;
        }

        public async Task VacancyApply(int vacancyId, int userId)
        {
            var response = await httpClient.GetAsync($"vacancy/apply-vacancy?vacancyId={vacancyId}&userId={userId}");

            if (response.IsSuccessStatusCode)
            {
            }
        }
    }
}

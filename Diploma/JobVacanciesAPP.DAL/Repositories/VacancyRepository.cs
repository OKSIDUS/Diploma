using JobVacanciesAPP.DAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Auth;
using JobVacanciesAPP.DAL.Models.Users;
using JobVacanciesAPP.DAL.Models.Vacancy;
using System;
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

        public async Task ChangeStatus(int vacancyId, int userId, string status)
        {
            var response = await httpClient.GetAsync($"applications/update-status?vacancyId={vacancyId}&userId={userId}&status={status}");

            if (response.IsSuccessStatusCode)
            {
            }

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

        public async Task<VacancyPage> GetAllVacancy(int page, int pageSize, bool isRecommendation, int userId, string keyword)
        {
            var url = $"vacancy/get-vacancies-page?page={page}&pageSize={pageSize}&isRecommendation={isRecommendation.ToString().ToLower()}&userId={userId}";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                url += $"&keyword={Uri.EscapeDataString(keyword)}";
            }
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyPage>();
                return vacancyPage;
            }

            return null;
        }

        public async Task<List<CandidateApplications>> GetCandidateApplications(int userId)
        {
            var response = await httpClient.GetAsync($"applications/get-user-applications?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var info = await response.Content.ReadFromJsonAsync<List<CandidateApplications>>();
                return info;
            }

            return new List<CandidateApplications>();
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

        public async Task<VacancyPage> GetRecommendetVacancies(int page, int pageSize, bool isRecommendation, int userId, string keyword)
        {
            var response = await httpClient.GetAsync($"vacancy/get-vacancies-page?page={page}&pageSize={pageSize}&isRecommendation={isRecommendation.ToString().ToLower()}&userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyPage>();
                return vacancyPage;
            }

            return null;
        }


        public async Task<VacancyPage> GetVacanciesForRecruiter(int page, int pageSize, bool isRecommendation, int userId, string keyword)
        {
            var url = $"vacancy/get-vacancies-page?page={page}&pageSize={pageSize}&isRecommendation={isRecommendation.ToString().ToLower()}&userId={userId}";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                url += $"&keyword={Uri.EscapeDataString(keyword)}";
            }
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyPage>();
                return vacancyPage;
            }

            return null;
        }

        public async Task<VacancyDTO> GetVacancy(int vacancyId)
        {
            var url = $"vacancy/get-info?id={vacancyId}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyDTO>();
                return vacancyPage;
            }

            return null;
        }

        public async Task<VacancyRecruiterDTO> GetVacancyInfoForRecruiter(int vacancyId)
        {
            var url = $"vacancy/get-recruiter-info?id={vacancyId}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var vacancyPage = await response.Content.ReadFromJsonAsync<VacancyRecruiterDTO>();
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

using JobVacanciesAPP.DAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Auth;
using JobVacanciesAPP.DAL.Models.Users;
using System.Net.Http.Json;
using System.Text.Json;

namespace JobVacanciesAPP.DAL.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApiSettings api;
        private HttpClient httpClient = new HttpClient();
        public UserProfileRepository(ApiSettings apiSetting)
        {
            api = apiSetting;
            httpClient.BaseAddress = new Uri(api.BaseUrl);
        }

        public async Task<bool> EditCandidateProfile(CandidateEdit candidate)
        {
            if (candidate != null)
            {
                var json = JsonSerializer.Serialize(candidate);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress + "user/edit-candidate-profile", content);
                string errorDetails = await response.Content.ReadAsStringAsync();

                Console.WriteLine(errorDetails);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> EditRecruiterProfile(RecruiterEdit recruiterProfile)
        {
            if (recruiterProfile != null)
            {
                var json = JsonSerializer.Serialize(recruiterProfile);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress + "user/edit-recruiter-profile", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<UserProfile> GetUserProfileAsync(int userId)
        {
            var response = await httpClient.GetAsync($"user/get-user-profile/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var userProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return userProfile;
            }

            return null;
        }

        public async Task<Skills> GetUserSkills(int userId)
        {
            var response = await httpClient.GetAsync($"user/get-user-skills/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var skills = await response.Content.ReadFromJsonAsync<Skills>();
                return skills;
            }

            return null;
        }

        public async Task SaveCandidateSkills(UserSkills skills)
        {
            if (skills != null)
            {
                var json = JsonSerializer.Serialize(skills);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress + "user/save-user-skills", content);

                if (response.IsSuccessStatusCode)
                {
                }
            }
        }
    }
}

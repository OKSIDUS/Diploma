using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllAsync();
        Task AddAsync(Tag tag);
        Task DeleteAsync(int id);
        Task<List<string>> GetUserTags(int userId);
        Task<List<string>> GetAllTagNames();

        Task SaveUserSkills(List<string> skills, int userId);
        Task SaveNewSkills(List<string> skills);
        Task SaveVacancySkills(List<string> skills, int vacancyId);
    }
}

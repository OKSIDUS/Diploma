using JobVacanciesAPI.BAL.DTOs.Tag;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface ITagService
    {
        Task<List<TagDTO>> GetAllAsync();
        Task AddAsync(TagDTO dto);
        Task DeleteAsync(int id);

        Task<UserTags> GetUserTags(int userId);
    }
}

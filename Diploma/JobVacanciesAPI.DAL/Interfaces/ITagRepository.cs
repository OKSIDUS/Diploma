using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllAsync();
        Task AddAsync(Tag tag);
        Task DeleteAsync(int id);
    }
}

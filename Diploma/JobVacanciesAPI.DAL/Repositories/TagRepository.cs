using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly JobVacancyDbContext _context;

        public TagRepository(JobVacancyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task AddAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetUserTags(int userId)
        {
            var tagIds = await _context.CandidateSkills.Where(x => x.UserId == userId).Select(x => x.TagId).ToListAsync();
            var tagNames = await _context.Tags
                .Where(t => tagIds.Contains(t.Id))
                .Select(t => t.Name)
                .ToListAsync();

            return tagNames;
        }

        public async Task<List<string>> GetAllTagNames()
        {
            return await _context.Tags.Select(x => x.Name).ToListAsync();
        }
    }
}

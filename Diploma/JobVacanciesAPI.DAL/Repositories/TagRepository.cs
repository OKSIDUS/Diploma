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

        public async Task SaveUserSkills(List<string> skills, int userId)
        {
            var cleanedSkills = skills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingTags = await _context.Tags
                .Where(t => cleanedSkills.Contains(t.Name))
                .ToListAsync();

            var existingTagNames = existingTags.Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var currentUserSkills = await _context.CandidateSkills
                .Where(ut => ut.UserId == userId)
                .ToListAsync();

            var newTagIds = existingTags
                .Where(t => cleanedSkills.Contains(t.Name, StringComparer.OrdinalIgnoreCase))
                .Select(t => t.Id)
                .ToHashSet();

            var tagsToRemove = currentUserSkills
                .Where(ut => !newTagIds.Contains(ut.TagId))
                .ToList();

            if (tagsToRemove.Any())
                _context.CandidateSkills.RemoveRange(tagsToRemove);

            var currentTagIds = currentUserSkills.Select(ut => ut.TagId).ToHashSet();
            var tagsToAdd = newTagIds
                .Where(tagId => !currentTagIds.Contains(tagId))
                .Select(tagId => new CandidateSkills
                {
                    UserId = userId,
                    TagId = tagId
                }).ToList();

            if (tagsToAdd.Any())
                await _context.CandidateSkills.AddRangeAsync(tagsToAdd);

            await _context.SaveChangesAsync();


        }

        public async Task SaveNewSkills(List<string> skills)
        {
           var existingTags = await _context.Tags
                .Where(t => skills.Contains(t.Name))
                .Select (t => t.Name)
                .ToListAsync();

            var newTags = skills.
                Where(t => !existingTags.Contains(t, StringComparer.OrdinalIgnoreCase))
                .Select(t => new Tag { Name = t })
                .ToList();

            if (newTags.Any())
            {
                await _context.Tags.AddRangeAsync(newTags);
                await _context.SaveChangesAsync();
            }

        }
    }
}

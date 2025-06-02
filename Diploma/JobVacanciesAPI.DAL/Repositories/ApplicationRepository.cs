using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly JobVacancyDbContext _context;

        public ApplicationRepository(JobVacancyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Candidate>> GetCandidatesByVacancyIdAsync(int vacancyId)
        {
            return await _context.Applications
                .Where(a => a.VacancyId == vacancyId)
                .Select(a => a.Candidate)
                .ToListAsync();
        }
    }
}

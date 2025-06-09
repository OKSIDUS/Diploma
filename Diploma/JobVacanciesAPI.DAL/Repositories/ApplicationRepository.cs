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

        public async Task<Application?> GetByIdAsync(int id)
        {
            return await _context.Applications.FindAsync(id);
        }

        public async Task AddAsync(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Application application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetStatus(int userID, int vacancyId)
        {
            return await _context.Applications.Where(a => a.CandidateId == userID && a.VacancyId == vacancyId).Select(a => a.Status).FirstOrDefaultAsync();
        }

        public async Task ChangeStatus(int vacancyId, int candidateId, string status)
        {
            var application = await _context.Applications.Where(a => a.VacancyId == vacancyId && a.CandidateId == candidateId).FirstOrDefaultAsync();
            if(application != null)
            {
                application.Status = status;
            }

            await _context.SaveChangesAsync();
        }
    }
}

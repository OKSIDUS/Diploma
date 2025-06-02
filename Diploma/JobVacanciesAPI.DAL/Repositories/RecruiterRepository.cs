using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Repositories
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly JobVacancyDbContext _context;

        public RecruiterRepository(JobVacancyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recruiter>> GetAllAsync()
        {
            return await _context.Recruiters.ToListAsync();
        }

        public async Task<Recruiter?> GetByIdAsync(int id)
        {
            return await _context.Recruiters.FindAsync(id);
        }

        public async Task AddAsync(Recruiter recruiter)
        {
            _context.Recruiters.Add(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recruiter recruiter)
        {
            _context.Recruiters.Update(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recruiter = await _context.Recruiters.FindAsync(id);
            if (recruiter != null)
            {
                _context.Recruiters.Remove(recruiter);
                await _context.SaveChangesAsync();
            }
        }
    }
}

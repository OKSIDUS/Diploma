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
            var recruiter =  await _context.Recruiters.Where(r => r.UserId == id).FirstOrDefaultAsync();
            return recruiter;
        }

        public async Task AddAsync(Recruiter recruiter)
        {
            _context.Recruiters.Add(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recruiter recruiter)
        {
            var oldRecruiter = await _context.Recruiters.Where(r => r.UserId == recruiter!.UserId).FirstOrDefaultAsync();
            if (oldRecruiter != null)
            {
                oldRecruiter.UserId = recruiter.UserId;
                oldRecruiter.Position = recruiter.Position;
                oldRecruiter.CompanyName = recruiter.CompanyName;
            }
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

        public async Task<string> GetRecruiterCompany(int userId)
        {
            return await _context.Recruiters.Where(r => r.UserId == userId).Select(r => r.CompanyName).FirstOrDefaultAsync();
        }

        public async Task<string> GetRecruiterCompanyById(int recruiterId)
        {
            return await _context.Recruiters.Where(r => r.Id == recruiterId).Select(r => r.CompanyName).FirstOrDefaultAsync();

        }
    }
}

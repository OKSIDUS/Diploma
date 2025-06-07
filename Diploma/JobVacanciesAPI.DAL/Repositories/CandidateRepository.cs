using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly JobVacancyDbContext _context;

        public CandidateRepository(JobVacancyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Candidate>> GetAllAsync()
        {
            return await _context.Candidates.ToListAsync();
        }

        public async Task<Candidate?> GetByIdAsync(int id)
        {
            return await _context.Candidates.Where(c => c.UserId == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            var oldCandidate = await _context.Candidates.Where(c => c.UserId == candidate.UserId).FirstOrDefaultAsync();
            if (oldCandidate != null)
            {
                oldCandidate.UserId = candidate.UserId;
                oldCandidate.Skills = candidate.Skills;
                oldCandidate.FullName = candidate.FullName;
                oldCandidate.DateOfBirth = candidate.DateOfBirth;
                oldCandidate.ResumeFilePath  = candidate.ResumeFilePath;
                oldCandidate.Experience = candidate.Experience;
            };
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
        }
    }
}

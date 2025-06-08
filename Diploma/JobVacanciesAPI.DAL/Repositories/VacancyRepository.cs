using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly IJobVacancyDbContext _context;

        public VacancyRepository(IJobVacancyDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Vacancy>> GetAllAsync()
        {
            return await _context.Vacancies.ToListAsync();
        }

        public async Task<Vacancy?> GetByIdAsync(int id)
        {
            return await _context.Vacancies.FindAsync(id);
        }

        public async Task<List<Vacancy>> GetByRecruiterIdAsync(int recruiterId)
        {
            var Id = await _context.Recruiters.Where(r => r.UserId == recruiterId).Select(r => r.Id).FirstOrDefaultAsync();
            return await _context.Vacancies
                .Where(v => v.RecruiterId == Id)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Vacancy vacancy)
        {
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();
            return vacancy.Id;
        }

        public async Task UpdateAsync(Vacancy vacancy)
        {
            _context.Vacancies.Update(vacancy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy != null)
            {
                _context.Vacancies.Remove(vacancy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Vacancy>> GetNewVacancies()
        {
            return await _context.Vacancies.Where(v => v.IsActive == true).OrderByDescending(v => v.CreatedAt).ToListAsync();
        }

        public async Task<List<Vacancy>> GetVacanciesByIds(List<int> ids)
        {
            return await _context.Vacancies.Where(v => ids.Contains(v.Id)).ToListAsync();
        }

        public async Task VacancyApply(int vacancyId, int candidateId)
        {
            var application = new Application
            {
                CandidateId = candidateId,
                Status = "Pending",
                SubmittedAt = DateTime.UtcNow,
                VacancyId = vacancyId,
            };

            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
    }
}

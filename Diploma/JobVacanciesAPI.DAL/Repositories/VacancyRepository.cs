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

        public async Task CreateAsync(Vacancy vacancy)
        {
            _context.Vacancies.Add(vacancy);
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

        public async Task<List<Vacancy>> GetAllAsync()
        {
            return await _context.Vacancies.ToListAsync();
        }

        public async Task<Vacancy> GetByIdAsync(int id)
        {
            return await _context.Vacancies.FindAsync(id);
        }

        public async Task UpdateAsync(Vacancy vacancy)
        {
            _context.Vacancies.Update(vacancy);
            await _context.SaveChangesAsync();
        }
    }
}

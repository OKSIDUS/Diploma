using JobVacanciesAPI.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Context
{
    public interface IJobVacancyDbContext
    {
        DbSet<Vacancy> Vacancies { get; }
    }
}

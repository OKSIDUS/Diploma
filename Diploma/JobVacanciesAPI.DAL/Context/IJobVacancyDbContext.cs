using JobVacanciesAPI.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Context
{
    public interface IJobVacancyDbContext
    {
        DbSet<Vacancy> Vacancies { get; }
        public DbSet<Application> Applications { get; }
        public DbSet<Recommendation> Recommendations { get; }
        public DbSet<Recruiter> Recruiters { get; }
        public DbSet<Tag> Tags { get; }
        public DbSet<User> Users { get; }
        public DbSet<VacancyTags> VacancyTags { get; }
        public DbSet<Candidate> Candidates { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

using JobVacanciesAPI.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Context
{
    public class JobVacancyDbContext : DbContext, IJobVacancyDbContext
    {
        public JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options) : base(options) { }

        public DbSet<Vacancy> Vacancies {  get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VacancyTags> VacancyTags { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public static string TablePrefix { get; set; } = "";
        public static string Schema { get; set; } = "dbo";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

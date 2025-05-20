using JobVacanciesAPI.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Context
{
    public class JobVacancyDbContext : DbContext, IJobVacancyDbContext
    {
        public JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options) : base(options) { }

        public DbSet<Vacancy> Vacancies {  get; set; }
        public static string TablePrefix { get; set; } = "";
        public static string Schema { get; set; } = "dbo";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

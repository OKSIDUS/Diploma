
using JobVacancies.Common;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.BAL.Services;
using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Interfaces;
using JobVacanciesAPI.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobVacanciesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<JobVacancyDbContext>(options =>
                options.UseSqlServer(defaultConnectionString));
            builder.Services.AddScoped<IJobVacancyDbContext>(provider => provider.GetService<JobVacancyDbContext>());

            //DAL
            builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();

            //BAL
            builder.Services.AddScoped<IVacancyService, VacancyService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

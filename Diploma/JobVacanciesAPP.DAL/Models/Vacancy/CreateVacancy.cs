using JobVacanciesAPP.DAL.Models.Users;

namespace JobVacanciesAPP.DAL.Models.Vacancy
{
    public class CreateVacancy
    {
        public int UserId { get; set; }
        public Skills Skills { get; set; }
        public Vacancy VacancyInfo { get; set;}
    }
}

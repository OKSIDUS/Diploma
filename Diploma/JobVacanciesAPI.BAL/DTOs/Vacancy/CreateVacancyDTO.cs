using JobVacanciesAPI.BAL.DTOs.Tag;
using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.BAL.DTOs.Vacancy
{
    public class CreateVacancyDTO
    {
        public int UserId { get; set; }
        public UserTags Skills { get; set; }
        public VacancyInfo VacancyInfo { get; set; }
    }
}

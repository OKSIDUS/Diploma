using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Vacancies")]
    public class Vacancy
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string Tags { get; set; }

        public int RecruiterId { get; set; }
    }
}

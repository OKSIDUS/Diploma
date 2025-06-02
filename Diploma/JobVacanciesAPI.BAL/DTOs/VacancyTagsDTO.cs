using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.BAL.Entity
{
    public class VacancyTagsDTO
    {
        public int VacancyId { get; set; }

        public int TagId { get; set; }
    }
}

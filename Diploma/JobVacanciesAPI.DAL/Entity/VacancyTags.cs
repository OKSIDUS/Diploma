using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("VacancyTags")]
    public class VacancyTags
    {
        public int VacancyId { get; set; }

        public int TagId { get; set; }
    }
}

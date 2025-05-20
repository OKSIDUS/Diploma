using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Vacancies")]
    public class Vacancy
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
    }
}

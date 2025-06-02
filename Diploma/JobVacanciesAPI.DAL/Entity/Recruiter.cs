using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Recruiters")]
    public class Recruiter
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }

        public int UserId { get; set; }
    }
}

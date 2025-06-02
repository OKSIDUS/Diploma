using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Recruiters")]
    public class Recruiter
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }

        public int UserId { get; set; }
    }
}

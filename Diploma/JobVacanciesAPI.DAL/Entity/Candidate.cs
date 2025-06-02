using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Candidates")]
    public class Candidate
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ResumeLink { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }

        public int UserId { get; set; }
    }
}

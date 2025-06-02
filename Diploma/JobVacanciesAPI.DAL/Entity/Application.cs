using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Applications")]
    public class Application
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; }
    }
}

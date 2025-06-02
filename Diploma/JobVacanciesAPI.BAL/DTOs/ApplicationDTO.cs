using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.BAL.Entity
{
    public class ApplicationDTO
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; }
    }
}

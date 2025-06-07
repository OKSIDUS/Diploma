using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("CandidateSkills")]
    public class CandidateSkills
    {
        public int UserId { get; set; }
        public int TagId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.BAL.Entity
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ResumeFilePath { get; set; }
        public string Skills { get; set; }
        public int Experience { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int UserId { get; set; }
    }
}

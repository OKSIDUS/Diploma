using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.BAL.Entity
{
    public class RecruiterDTO
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }

        public int UserId { get; set; }
    }
}

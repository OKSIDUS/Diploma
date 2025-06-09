namespace JobVacanciesAPP.DAL.Models.Users
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public int Experience { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public List<string> Skills { get; set; }
    }
}

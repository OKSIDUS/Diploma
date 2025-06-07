namespace JobVacanciesAPP.Models
{
    public class EditCandidateProfileViewModel
    {
        public string FullName { get; set; }
        public string ResumeFilePath { get; set; }
        public string Skills { get; set; }
        public int Experience { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
    }
}

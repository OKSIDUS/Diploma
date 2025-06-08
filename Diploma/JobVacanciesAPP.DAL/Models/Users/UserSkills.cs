namespace JobVacanciesAPP.DAL.Models.Users
{
    public class UserSkills
    {
        public int UserId { get; set; }
        public List<string> SelectedTags { get; set; } = new();
        public List<string> AllAvailableTags { get; set; } = new();
    }
}

namespace JobVacanciesAPP.DAL.Models.Users
{
    public class Skills
    {
        public List<string> SelectedTags { get; set; } = new();
        public List<string> AllAvailableTags { get; set; } = new();
    }
}

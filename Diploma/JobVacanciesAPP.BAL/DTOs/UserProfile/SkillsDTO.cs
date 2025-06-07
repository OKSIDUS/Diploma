namespace JobVacanciesAPP.BAL.DTOs.UserProfile
{
    public class SkillsDTO
    {
        public List<string> SelectedTags { get; set; } = new();
        public List<string> AllAvailableTags { get; set; } = new();
    }
}

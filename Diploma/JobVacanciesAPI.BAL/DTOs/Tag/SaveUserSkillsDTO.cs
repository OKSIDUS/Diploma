namespace JobVacanciesAPI.BAL.DTOs.Tag
{
    public class SaveUserSkillsDTO
    {
        public int UserId { get; set; }
        public List<string> SelectedTags { get; set; } = new();
        public List<string> AllAvailableTags { get; set; } = new();
    }
}

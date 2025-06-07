namespace JobVacanciesAPI.BAL.DTOs.Tag
{
    public class UserTags
    {
        public List<string> SelectedTags { get; set; } = new();
        public List<string> AllAvailableTags { get; set; } = new();
    }
}

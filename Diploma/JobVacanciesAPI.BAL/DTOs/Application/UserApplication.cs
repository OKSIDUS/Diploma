namespace JobVacanciesAPI.BAL.DTOs.Application
{
    public class UserApplication
    {
        public int VacancyId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }
    }
}

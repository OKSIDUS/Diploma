namespace JobVacanciesAPI.BAL.DTOs.Vacancy
{
    public class VacancyPage
    {
        //public int UserId { get; set; }
       // public bool IsRecommendation { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; }
        public string Keyword { get; set; } = string.Empty;
        public List<VacancyShortInfo> Vacancies { get; set; }
    }
}

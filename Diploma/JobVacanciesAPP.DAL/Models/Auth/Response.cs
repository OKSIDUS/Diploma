namespace JobVacanciesAPP.DAL.Models.Auth
{
    public class Response
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
    }
}

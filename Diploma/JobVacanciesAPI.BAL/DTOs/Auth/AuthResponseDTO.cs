namespace JobVacanciesAPI.BAL.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
    }
}

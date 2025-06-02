namespace JobVacanciesAPI.BAL.Entity
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}

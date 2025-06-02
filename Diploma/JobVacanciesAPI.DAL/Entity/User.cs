using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}

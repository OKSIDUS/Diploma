using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.BAL.Entity
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

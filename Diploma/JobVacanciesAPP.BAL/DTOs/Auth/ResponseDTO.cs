using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobVacanciesAPP.BAL.DTOs.Auth
{
    public class ResponseDTO
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
    }
}

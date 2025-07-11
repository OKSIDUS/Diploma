﻿using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacanciesAPI.DAL.Entity
{
    [Table("Recommendations")]
    public class Recommendation
    {
        public int Id { get; set; }

        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public double Score { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}

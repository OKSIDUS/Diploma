using Microsoft.ML.Data;

namespace JobVacancies.RecommendationSystem.Model
{
    public class CandidateVacancyInteraction
    {
        [LoadColumn(0)]
        public uint CandidateId { get; set; }
        [LoadColumn(1)]
        public uint VacancyId { get; set; }
        [LoadColumn(2)]
        public float Rating { get; set; }
    }
}

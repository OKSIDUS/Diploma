using JobVacancies.RecommendationSystem.Model;
using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace JobVacancies.RecommendationSystem.Services
{
    public class RecommendationService
    {
        private readonly MLContext _mlContext;
        private RecommendationModel _recommendationModel = new RecommendationModel();
        private ITransformer _model;
        private readonly IJobVacancyDbContext _dbContext;

        public RecommendationService(MLContext mLContext, IJobVacancyDbContext dbContext)
        {
            _mlContext = mLContext;
            _dbContext = dbContext;
        }

        private async Task ComputeRecommendation()
        {
            var candidateIds = await _dbContext.Candidates.ToListAsync();
            var vacancyIds = await _dbContext.Vacancies.Select(c => c.Id).ToListAsync();

            foreach( var candidateId in candidateIds)
            {
                foreach (var vacancyId in vacancyIds)
                {
                    var candidateSkillIds = await _dbContext.CandidateSkills.Where(s => s.UserId == candidateId.UserId).Select(s => s.TagId).ToListAsync();
                    var candidateSkills = await _dbContext.Tags.Where(t => candidateSkillIds.Contains(t.Id)).Select(t => t.Name).ToListAsync();
                    var vacancySkillIds = await _dbContext.VacancyTags.Where(v => v.VacancyId == vacancyId).Select(v => v.TagId).ToListAsync();
                    var vacancySkills = await _dbContext.Tags.Where(t => vacancySkillIds.Contains(t.Id)).Select(t => t.Name).ToListAsync();

                    var score = ComputeCosineSimilarity(candidateSkills, vacancySkills);

                    var existingRecommendation = await _dbContext.Recommendations
                        .FirstOrDefaultAsync(r => r.CandidateId == candidateId.Id && r.VacancyId == vacancyId);

                    if (existingRecommendation != null)
                    {
                        existingRecommendation.Score = score;
                        _dbContext.Recommendations.Update(existingRecommendation);
                    }
                    else
                    {
                        var newRecommendation = new Recommendation
                        {
                            CandidateId = candidateId.Id,
                            VacancyId = vacancyId,
                            Score = score
                        };
                        _dbContext.Recommendations.Add(newRecommendation);
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<List<(int VacancyId, float Score)>> GetRecommendedVacancies(int candidateId)
        {
             await ComputeRecommendation();

            var applications = await _dbContext.Applications
                .Select(a => new ApplicationData
                {
                    CandidateId = a.CandidateId,
                    VacancyId = a.VacancyId,
                    Label = a.Status == "Accept" ? 1f : (a.Status == "Pending" ? 0.5f : 0f)
                })
                .ToListAsync();

            

            var declinedVacancies = await _dbContext.Applications
                .Where(a => a.CandidateId == candidateId && a.Status == "Decline")
                .Select(a => a.VacancyId)
                .ToListAsync();

            var similarities = await _dbContext.Recommendations
                .Where(s => s.CandidateId == candidateId && !declinedVacancies.Contains(s.VacancyId))
                .ToListAsync();

            if (!applications.Any())
            {
                return similarities
                    .OrderByDescending(s => s.Score)
                    .Take(10)
                    .Select(s => (s.VacancyId, (float)s.Score))
                    .ToList();
            }
            var model = new RecommendationModel();
            model.Train(applications);
            model.Save("model.zip");

            var recommendations = new List<(int VacancyId, float Score)>();

            foreach (var sim in similarities)
            {
                if (sim.Score < 0.1f)
                    continue;

                float mlScore = _recommendationModel.Predict(candidateId, sim.VacancyId);

                float finalScore = 0.5f * (float)sim.Score + 0.5f * mlScore;

                recommendations.Add((sim.VacancyId, finalScore));
            }

            return recommendations.OrderByDescending(r => r.Score).Take(10).ToList();
        }


        private float ComputeCosineSimilarity(List<string> candidateSkills, List<string> vacancySkills)
        {
            if (candidateSkills == null || vacancySkills == null || candidateSkills.Count == 0 || vacancySkills.Count == 0)
                return 0f;

            var intersectionCount = candidateSkills.Intersect(vacancySkills, StringComparer.OrdinalIgnoreCase).Count();
            var denominator = Math.Sqrt(candidateSkills.Count * vacancySkills.Count);

            return denominator == 0 ? 0f : (float)(intersectionCount / denominator);
        }



    }
}

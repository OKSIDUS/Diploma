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
        private ITransformer _model;
        private readonly IJobVacancyDbContext _dbContext;

        public RecommendationService(MLContext mLContext, IJobVacancyDbContext dbContext)
        {
            _mlContext = mLContext;
            _dbContext = dbContext;
        }

        public async Task ComputeRecommendation()
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

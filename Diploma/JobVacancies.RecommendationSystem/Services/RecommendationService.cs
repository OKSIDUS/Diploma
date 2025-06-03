using JobVacancies.RecommendationSystem.Model;
using JobVacanciesAPI.DAL.Context;
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

        public async Task TrainModelAsync()
        {
            var interactions = await _dbContext.Applications
               .Select(app => new CandidateVacancyInteraction
               {
                   CandidateId = (uint)app.CandidateId, 
                   VacancyId = (uint)app.VacancyId,     
                   Rating = (app.Status == "Approved") ? 1.0f :
                            (app.Status == "Pending") ? 0.5f :
                            (app.Status == "Declined") ? 0.1f :
                            0.0f
               })
               .ToListAsync();

            if (!interactions.Any())
            {
                return;
            }

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(interactions);

            var dataProcessingPipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                                            inputColumnName: nameof(CandidateVacancyInteraction.CandidateId),
                                            outputColumnName: "CandidateId_Key")
                                        .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                                            inputColumnName: nameof(CandidateVacancyInteraction.VacancyId),
                                            outputColumnName: "VacancyId_Key"));

            var transformedData = dataProcessingPipeline.Fit(dataView).Transform(dataView);

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "VacancyId_Key",   
                MatrixRowIndexColumnName = "CandidateId_Key",    
                LabelColumnName = nameof(CandidateVacancyInteraction.Rating),
                NumberOfIterations = 20,
                LearningRate = 0.01f
            };

            var estimator = _mlContext.Recommendation().Trainers.MatrixFactorization(options);

            _model = estimator.Fit(transformedData);

            _mlContext.Model.Save(_model, transformedData.Schema, "recommendation_model.zip");
        }


        public void LoadModel(string modelPath)
        {
            if(File.Exists(modelPath))
            {
                _model = _mlContext.Model.Load(modelPath, out var modelSchema);
            }
        }

        public async Task<IEnumerable<Tuple<int, float>>> GetRecommendationsAsync(int candidateId, int numberOfRecommendations = 10)
        {
            if (_model == null)
            {
                throw new InvalidOperationException("Recommendation model not found!");
            }

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CandidateVacancyInteraction, CandidateRecommendationPrediction>(_model);

            var allActiveVacancyIds = await _dbContext.Vacancies
                .Where(v => v.IsActive)
                .Select(v => v.Id)
                .ToListAsync();

            var appliedVacancyIds = await _dbContext.Applications
                .Where(a => a.CandidateId == candidateId)
                .Select(a => a.VacancyId)
                .ToListAsync();

            var vacanciesToPredict = allActiveVacancyIds.Except(appliedVacancyIds).ToList();

            var recommendations = new List<Tuple<int, float>>();

            foreach (var vacancyId in vacanciesToPredict)
            {
                var prediction = predictionEngine.Predict(
                    new CandidateVacancyInteraction
                    {
                        CandidateId = (uint)candidateId,
                        VacancyId = (uint)vacancyId,
                    });

                recommendations.Add(Tuple.Create(vacancyId, prediction.Score));

                
            }

            return recommendations.OrderByDescending(r => r.Item2)
                    .Take(numberOfRecommendations);
        }
    }
}

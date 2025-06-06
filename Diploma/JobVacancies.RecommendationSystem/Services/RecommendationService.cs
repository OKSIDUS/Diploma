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
            // 1. Prepare data from DB
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
                // Log a warning or handle gracefully if no data to train
                Console.WriteLine("No application data found to train the recommendation model.");
                return;
            }

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(interactions);

            // 2. Define the data processing pipeline (transforms)
            var dataProcessingPipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                                                inputColumnName: nameof(CandidateVacancyInteraction.CandidateId),
                                                outputColumnName: "CandidateId_Key")
                                        .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                                                inputColumnName: nameof(CandidateVacancyInteraction.VacancyId),
                                                outputColumnName: "VacancyId_Key"));

            // 3. Define the trainer options
            var trainerOptions = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "VacancyId_Key",
                MatrixRowIndexColumnName = "CandidateId_Key",
                LabelColumnName = nameof(CandidateVacancyInteraction.Rating),
                NumberOfIterations = 20,
                LearningRate = 0.01f
            };

            // 4. Append the trainer to the data processing pipeline
            // This creates a single, complete training pipeline
            var trainingPipeline = dataProcessingPipeline.Append(_mlContext.Recommendation().Trainers.MatrixFactorization(trainerOptions));

            // 5. Fit the entire pipeline to the data
            // _model now contains both the transformations and the trained Matrix Factorization model.
            _model = trainingPipeline.Fit(dataView);

            // 6. Save the trained model (the full pipeline)
            // The schema passed here should be the schema of the data *after* transformations,
            // which ML.NET usually handles automatically when saving a full pipeline.
            // A simple way is to get it from the fitted model's output schema, or if you save the full pipeline.
            // When saving a full pipeline, ML.NET usually infers the necessary input schema.
            // However, if your _model is the result of trainingPipeline.Fit(dataView), then _model.Schema
            // will represent the output schema of the entire pipeline, not necessarily the input for prediction.
            // For _mlContext.Model.Save, it expects the schema that defines the data structure *before* the model was fitted.
            // The common practice is to pass the schema of the data *entering* the training pipeline for consistency.
            // In this case, `dataView.Schema` represents the original input schema.
            _mlContext.Model.Save(_model, dataView.Schema, "recommendation_model.zip");
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

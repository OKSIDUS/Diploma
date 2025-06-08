using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace JobVacancies.RecommendationSystem.Model
{
    public class RecommendationModel
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private PredictionEngine<ApplicationData, ApplicationPrediction> _predictionEngine;

        public RecommendationModel()
        {
            _mlContext = new MLContext(seed: 1);
        }

        public void Train(IEnumerable<ApplicationData> data)
        {


            var trainingDataView = _mlContext.Data.LoadFromEnumerable(data);

            var pipeline = _mlContext.Transforms.Conversion
                    .MapValueToKey(nameof(ApplicationData.CandidateId))
                .Append(_mlContext.Transforms.Conversion
                    .MapValueToKey(nameof(ApplicationData.VacancyId)))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = nameof(ApplicationData.CandidateId),
                    MatrixRowIndexColumnName = nameof(ApplicationData.VacancyId),
                    LabelColumnName = nameof(ApplicationData.Label),
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                }));

            _model = pipeline.Fit(trainingDataView);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ApplicationData, ApplicationPrediction>(_model);
        }

        public float Predict(int candidateId, int vacancyId)
        {
            var input = new ApplicationData
            {
                CandidateId = candidateId,
                VacancyId = vacancyId
            };

            return _predictionEngine.Predict(input).Score;
        }

        public void Save(string modelPath)
        {
            using var fileStream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
            _mlContext.Model.Save(_model, null, fileStream);
        }

        public void Load(string modelPath)
        {
            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _model = _mlContext.Model.Load(stream, out var schema);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ApplicationData, ApplicationPrediction>(_model);
        }
    }


    public class ApplicationPrediction
    {
        public float Score { get; set; }
    }
}

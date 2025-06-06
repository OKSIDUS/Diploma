using JobVacancies.RecommendationSystem.Model;
using JobVacancies.RecommendationSystem.Services;
using JobVacancies.Tests.Helper;
using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace JobVacancies.Tests.Services
{
    public class RecommendationServiceTests
    {
        private readonly Mock<IJobVacancyDbContext> _mockDbContext;
        private readonly MLContext _mlContext;
        private readonly RecommendationService _service;

        public RecommendationServiceTests()
        {
            _mockDbContext = new Mock<IJobVacancyDbContext>();
            _mlContext = new MLContext();
            _service = new RecommendationService(_mlContext, _mockDbContext.Object);
        }

        [Fact]
        public async Task TrainModelAsync_WithData_TrainsSuccessfully()
        {
            _mockDbContext.Setup(c => c.Applications).Returns(DbSetMockHelper.CreateMockDbSet(new List<Application>
            {
                new Application { CandidateId = 1, VacancyId = 10, Status = "Approved" },
                new Application { CandidateId = 2, VacancyId = 20, Status = "Approved" }
            }).Object);

            await _service.TrainModelAsync();

            Assert.True(true); // просто щоб переконатися, що не впало
        }

        [Fact]
        public async Task GetRecommendationsAsync_ModelNotTrained_ThrowsException()
        {
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetRecommendationsAsync(1));

            Assert.Equal("Recommendation model not found!", ex.Message);
        }

        
    }
}

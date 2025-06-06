using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Vacancy;
using JobVacanciesAPI.BAL.Services;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace JobVacancies.Tests.Services
{
    public class VacancyServiceTests
    {
        private readonly Mock<IVacancyRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly VacancyService _service;

        public VacancyServiceTests()
        {
            _mockRepo = new Mock<IVacancyRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new VacancyService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedVacancies()
        {
            // Arrange
            var vacancies = new List<Vacancy>
            {
                new Vacancy { Id = 1, Title = "Test Vacancy 1" },
                new Vacancy { Id = 2, Title = "Test Vacancy 2" }
            };

            var vacancyDTOs = new List<VacancyDTO>
            {
                new VacancyDTO { Id = 1, Title = "Test Vacancy 1" },
                new VacancyDTO { Id = 2, Title = "Test Vacancy 2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(vacancies);
            _mockMapper.Setup(m => m.Map<List<VacancyDTO>>(vacancies)).Returns(vacancyDTOs);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Vacancy 1", result[0].Title);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Vacancy?)null);

            var result = await _service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            var called = false;
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                .Callback(() => called = true)
                .Returns(Task.CompletedTask);

            await _service.DeleteAsync(1);

            Assert.True(called);
        }
    }
}

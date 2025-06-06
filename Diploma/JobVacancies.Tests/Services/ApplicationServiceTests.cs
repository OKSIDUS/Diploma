using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Application;
using JobVacanciesAPI.BAL.Services;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace JobVacancies.Tests.Services
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ApplicationService _service;

        public ApplicationServiceTests()
        {
            _mockRepo = new Mock<IApplicationRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ApplicationService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedApplication()
        {
            // Arrange
            var app = new Application { Id = 1, CandidateId = 10, VacancyId = 20, Status = "Pending" };
            var appDTO = new ApplicationDTO { Id = 1, CandidateId = 10, VacancyId = 20, Status = "Pending" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(app);
            _mockMapper.Setup(m => m.Map<ApplicationDTO>(app)).Returns(appDTO);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.CandidateId);
            Assert.Equal("Pending", result.Status);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAndSetsDefaults()
        {
            var dto = new ApplicationDTO
            {
                Id = 0,
                CandidateId = 2,
                VacancyId = 5
                // Status and SubmittedAt will be set inside service
            };

            var entity = new Application
            {
                CandidateId = 2,
                VacancyId = 5,
                Status = "Pending"
            };

            _mockMapper.Setup(m => m.Map<Application>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Application>())).Returns(Task.CompletedTask);

            await _service.AddAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Application>(a =>
                a.CandidateId == 2 &&
                a.VacancyId == 5 &&
                a.Status == "Pending"
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_UpdatesCorrectly()
        {
            var existing = new Application { Id = 1, Status = "Pending" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Application>())).Returns(Task.CompletedTask);

            await _service.UpdateStatusAsync(1, "Approved");

            Assert.Equal("Approved", existing.Status);
            _mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenNotFound_DoesNothing()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Application?)null);

            await _service.UpdateStatusAsync(999, "Declined");

            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Application>()), Times.Never);
        }
    }
}

using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Recruiter;
using JobVacanciesAPI.BAL.Services;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace JobVacancies.Tests.Services
{
    public class RecruiterServiceTests
    {
        private readonly Mock<IRecruiterRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RecruiterService _service;

        public RecruiterServiceTests()
        {
            _mockRepo = new Mock<IRecruiterRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new RecruiterService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsRecruiterDTOs()
        {
            var recruiters = new List<Recruiter>
            {
                new Recruiter { Id = 1, CompanyName = "Company A", Position = "HR" },
                new Recruiter { Id = 2, CompanyName = "Company B", Position = "Lead" }
            };

            var dtos = new List<RecruiterDTO>
            {
                new RecruiterDTO { Id = 1, CompanyName = "Company A", Position = "HR" },
                new RecruiterDTO { Id = 2, CompanyName = "Company B", Position = "Lead" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(recruiters);
            _mockMapper.Setup(m => m.Map<List<RecruiterDTO>>(recruiters)).Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Company A", result[0].CompanyName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRecruiterDTO()
        {
            var entity = new Recruiter { Id = 10, CompanyName = "Google", Position = "Tech Recruiter" };
            var dto = new RecruiterDTO { Id = 10, CompanyName = "Google", Position = "Tech Recruiter" };

            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<RecruiterDTO>(entity)).Returns(dto);

            var result = await _service.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("Google", result.CompanyName);
        }

        [Fact]
        public async Task AddAsync_CallsRepository()
        {
            var dto = new RecruiterDTO { CompanyName = "Meta", Position = "HR" };
            var entity = new Recruiter { CompanyName = "Meta", Position = "HR" };

            _mockMapper.Setup(m => m.Map<Recruiter>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);

            await _service.AddAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Recruiter>(r => r.CompanyName == "Meta")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            var dto = new RecruiterDTO { Id = 5, CompanyName = "UpdatedCo", Position = "Manager" };
            var entity = new Recruiter { Id = 5, CompanyName = "UpdatedCo", Position = "Manager" };

            _mockMapper.Setup(m => m.Map<Recruiter>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            await _service.UpdateAsync(dto);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Recruiter>(r => r.Id == 5 && r.CompanyName == "UpdatedCo")), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            _mockRepo.Setup(r => r.DeleteAsync(3)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(3);

            _mockRepo.Verify(r => r.DeleteAsync(3), Times.Once);
        }
    }
}

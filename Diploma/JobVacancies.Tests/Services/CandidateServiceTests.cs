using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Candidate;
using JobVacanciesAPI.BAL.Services;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace JobVacancies.Tests.Services
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CandidateService _service;

        public CandidateServiceTests()
        {
            _mockRepo = new Mock<ICandidateRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CandidateService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCandidateDTOList()
        {
            var candidates = new List<Candidate>
            {
                new Candidate { Id = 1, FullName = "John Smith", Experience = 2, Skills = "C#, SQL" },
                new Candidate { Id = 2, FullName = "Alice Brown", Experience = 3, Skills = "Python" }
            };

            var dtoList = new List<CandidateDTO>
            {
                new CandidateDTO { Id = 1, FullName = "John Smith", Experience = 2, Skills = "C#, SQL" },
                new CandidateDTO { Id = 2, FullName = "Alice Brown", Experience = 3, Skills = "Python" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(candidates);
            _mockMapper.Setup(m => m.Map<List<CandidateDTO>>(candidates)).Returns(dtoList);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("John Smith", result[0].FullName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCandidateDTO()
        {
            var candidate = new Candidate { Id = 5, FullName = "Test Person", Experience = 1, Skills = "Java" };
            var dto = new CandidateDTO { Id = 5, FullName = "Test Person", Experience = 1, Skills = "Java" };

            _mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(candidate);
            _mockMapper.Setup(m => m.Map<CandidateDTO>(candidate)).Returns(dto);

            var result = await _service.GetByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryWithMappedEntity()
        {
            var dto = new CandidateDTO { Id = 0, FullName = "New Candidate", Experience = 0, Skills = "Go" };
            var entity = new Candidate { FullName = "New Candidate", Experience = 0, Skills = "Go" };

            _mockMapper.Setup(m => m.Map<Candidate>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);

            await _service.AddAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Candidate>(c => c.FullName == "New Candidate")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesMappedEntity()
        {
            var dto = new CandidateDTO { Id = 3, FullName = "Updated", Experience = 5, Skills = "Rust" };
            var entity = new Candidate { Id = 3, FullName = "Updated", Experience = 5, Skills = "Rust" };

            _mockMapper.Setup(m => m.Map<Candidate>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            await _service.UpdateAsync(dto);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Candidate>(c => c.Id == 3 && c.FullName == "Updated")), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            _mockRepo.Setup(r => r.DeleteAsync(10)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(10);

            _mockRepo.Verify(r => r.DeleteAsync(10), Times.Once);
        }

        
    }
}

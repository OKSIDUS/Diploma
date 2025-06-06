using JobVacanciesAPI.BAL.DTOs.Auth;
using JobVacanciesAPI.BAL.Services;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace JobVacancies.Tests.Services
{

    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockConfig = new Mock<IConfiguration>();

            // Мокаємо JWT конфіг
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("super_secure_jwt_key_that_is_very_long_123!");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("testIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("testAudience");

            _service = new AuthService(_mockRepo.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task LoginAsync_WithCorrectCredentials_ReturnsToken()
        {
            var user = new User
            {
                Id = 1,
                Email = "user@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = "Candidate"
            };

            _mockRepo.Setup(r => r.GetByEmail("user@test.com")).ReturnsAsync(user);

            var result = await _service.LoginAsync(new LoginDTO
            {
                Email = "user@test.com",
                Password = "123456"
            });

            Assert.NotNull(result);
            Assert.Equal("Candidate", result!.Role);
            Assert.True(result.Token.Length > 10);
        }

        [Fact]
        public async Task LoginAsync_WithWrongPassword_ReturnsNull()
        {
            var user = new User
            {
                Id = 1,
                Email = "user@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = "Candidate"
            };

            _mockRepo.Setup(r => r.GetByEmail("user@test.com")).ReturnsAsync(user);

            var result = await _service.LoginAsync(new LoginDTO
            {
                Email = "user@test.com",
                Password = "wrongpass"
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterAsync_WithNewEmail_ReturnsToken()
        {
            _mockRepo.Setup(r => r.EmailExistsAsync("new@test.com")).ReturnsAsync(false);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _service.RegisterAsync(new RegisterDTO
            {
                Email = "new@test.com",
                Password = "123456",
                Role = "Recruiter"
            });

            Assert.NotNull(result);
            Assert.Equal("Recruiter", result!.Role);
            Assert.True(result.Token.Length > 10);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ReturnsNull()
        {
            _mockRepo.Setup(r => r.EmailExistsAsync("exist@test.com")).ReturnsAsync(true);

            var result = await _service.RegisterAsync(new RegisterDTO
            {
                Email = "exist@test.com",
                Password = "pass",
                Role = "Admin"
            });

            Assert.Null(result);
        }

        
    }
}

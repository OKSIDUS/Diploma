using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Application;
using JobVacanciesAPI.BAL.DTOs.Candidate;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository repo, IMapper mapper, IUserRepository userRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<CandidateDTO>> GetCandidatesByVacancyIdAsync(int vacancyId)
        {
            var candidates = await _repo.GetCandidatesByVacancyIdAsync(vacancyId);
            return _mapper.Map<List<CandidateDTO>>(candidates);
        }

        public async Task<ApplicationDTO?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ApplicationDTO>(entity);
        }

        public async Task AddAsync(ApplicationDTO dto)
        {
            var entity = _mapper.Map<Application>(dto);
            entity.SubmittedAt = DateTime.UtcNow;
            entity.Status = "Pending";
            await _repo.AddAsync(entity);
        }

        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                entity.Status = newStatus;
                await _repo.UpdateAsync(entity);
            }
        }

        public async Task ChangeStatus(int vacancyId, int userId, string newStatus)
        {
            //var candidateId = await _userRepository.GetCandidateId(userId);
            await _repo.ChangeStatus(vacancyId, userId, newStatus);
        }
    }
}

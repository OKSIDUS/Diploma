using AutoMapper;
using JobVacanciesAPI.BAL.Entity;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _repo;
        private readonly IMapper _mapper;

        public CandidateService(ICandidateRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CandidateDTO>> GetAllAsync()
        {
            var candidates = await _repo.GetAllAsync();
            return _mapper.Map<List<CandidateDTO>>(candidates);
        }

        public async Task<CandidateDTO?> GetByIdAsync(int id)
        {
            var candidate = await _repo.GetByIdAsync(id);
            return candidate == null ? null : _mapper.Map<CandidateDTO>(candidate);
        }

        public async Task AddAsync(CandidateDTO dto)
        {
            var entity = new Candidate
            {
                DateOfBirth = dto.DateOfBirth,
                Experience = dto.Experience,
                FullName = dto.FullName,
                Skills = dto.Skills,
                ResumeFilePath = dto.ResumeFilePath,
                UserId = dto.UserId,
            };
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(CandidateDTO dto)
        {
            var entity = _mapper.Map<Candidate>(dto);
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}

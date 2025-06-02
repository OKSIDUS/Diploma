using AutoMapper;
using JobVacanciesAPI.BAL.Entity;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repo;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CandidateDTO>> GetCandidatesByVacancyIdAsync(int vacancyId)
        {
            var candidates = await _repo.GetCandidatesByVacancyIdAsync(vacancyId);
            return _mapper.Map<List<CandidateDTO>>(candidates);
        }
    }
}

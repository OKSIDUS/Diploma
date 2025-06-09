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
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public ApplicationService(IApplicationRepository repo, IMapper mapper, IUserRepository userRepository, IVacancyRepository vacancyRepo, IRecruiterRepository recruiterRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _userRepository = userRepository;
            _vacancyRepository = vacancyRepo;
            _recruiterRepository = recruiterRepository;
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

        public async Task<IEnumerable<UserApplication>> GetUserApplications(int userId)
        {
            var candidateId = await _userRepository.GetCandidateId(userId);
            var applications = await _repo.GetApplicationsByUserId(candidateId);
            var vacancyIds = applications.Select(a => a.VacancyId).ToList();
            List<UserApplication> users = new List<UserApplication>();

            foreach(var app in applications)
            {
                var vacancy = await _vacancyRepository.GetByIdAsync(app.VacancyId);
                var company = await _recruiterRepository.GetRecruiterCompanyById(vacancy.RecruiterId);
                users.Add(new UserApplication
                {
                    VacancyId = vacancy.Id,
                    Company = company,
                    AppliedDate = app.SubmittedAt,
                    Status = app.Status,
                    Title = vacancy.Title

                });
            }

            return users;
        }
    }
}

using AutoMapper;
using JobVacancies.RecommendationSystem.Services;
using JobVacanciesAPI.BAL.DTOs.Candidate;
using JobVacanciesAPI.BAL.DTOs.Vacancy;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly RecommendationService _recommendationService;
        private readonly IVacancyRepository _repo;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IApplicationRepository _applicationRepository;

        public VacancyService(IVacancyRepository repo, IMapper mapper, IRecruiterRepository recruiterRepository, ITagRepository tagRepository, IUserRepository userRepository, RecommendationService recommendationService, IApplicationRepository applicationRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _recruiterRepository = recruiterRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _recommendationService = recommendationService;
            _applicationRepository = applicationRepository;
        }

        public async Task<List<VacancyDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<VacancyDTO>>(list);
        }

        public async Task<VacancyDTO?> GetByIdAsync(int id)
        {
            var vacancy = await _repo.GetByIdAsync(id);
            return vacancy == null ? null : _mapper.Map<VacancyDTO>(vacancy);
        }

        public async Task<List<VacancyDTO>> GetByRecruiterAsync(int recruiterId)
        {
            var list = await _repo.GetByRecruiterIdAsync(recruiterId);
            return _mapper.Map<List<VacancyDTO>>(list);
        }

        public async Task AddAsync(VacancyDTO dto)
        {
            var entity = new Vacancy
            {
                CreatedAt = dto.CreatedAt,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Location = dto.Location,
                RecruiterId = dto.RecruiterId,
                Requirements = dto.Requirements,
                SalaryMax = dto.SalaryMax,
                SalaryMin = dto.SalaryMin,
                Title = dto.Title,
            };
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(VacancyDTO dto)
        {
            var entity = _mapper.Map<Vacancy>(dto);
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task CreateVacancy(CreateVacancyDTO vacancyDTO)
        {
            if(vacancyDTO != null)
            {
                var recruiter = await _recruiterRepository.GetByIdAsync(vacancyDTO.UserId);

                int vacancyId = await _repo.AddAsync(new Vacancy
                {
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    SalaryMax = vacancyDTO.VacancyInfo.SalaryMax,
                    SalaryMin = vacancyDTO.VacancyInfo.SalaryMin,
                    Description = vacancyDTO.VacancyInfo.Description,
                    Location = vacancyDTO.VacancyInfo.Location,
                    Requirements = vacancyDTO.VacancyInfo.Requirements,
                    Title = vacancyDTO.VacancyInfo.Title,
                    RecruiterId = recruiter.Id
                });

                if (vacancyDTO.Skills != null)
                {
                    var newTags = vacancyDTO.Skills.SelectedTags
                        .Where(tag => !vacancyDTO.Skills.AllAvailableTags.Contains(tag))
                        .ToList();

                    await _tagRepository.SaveNewSkills(newTags);
                    await _tagRepository.SaveVacancySkills(vacancyDTO.Skills.SelectedTags, vacancyId);

                }


            }
        }

        public async Task<VacancyPage> GetVacancyPage(int page, int pageSize, bool isRecommendation, int userId, string keyword)
        {
            VacancyPage vacancy = new VacancyPage();
            vacancy.Page = page;
            vacancy.PageSize = pageSize;
            vacancy.Vacancies = new();
            if (userId > 0)
            {
                var userRole = await _userRepository.GetUserRole(userId);

                if (userRole == "Recruiter")
                {
                    var company = await _recruiterRepository.GetRecruiterCompany(userId);
                    var vacancies = await _repo.GetByRecruiterIdAsync(userId);
                    IEnumerable<Vacancy> filtered = vacancies;

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        filtered = filtered.Where(v => v.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                    }

                    var filteredList = filtered.ToList();
                    var totalItems = PageCount(filteredList.Count, pageSize);

                    var items = filteredList
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                    vacancy.PageCount = totalItems;
                    vacancy.Vacancies = items.Select(i => new VacancyShortInfo
                    {
                        Description = i.Description,
                        Location = i.Location,
                        Title = i.Title,
                        Id = i.Id,
                        Company ="company"
                    }).ToList();
                }
                if (userRole == "Candidate")
                {
                    if (isRecommendation)
                    {
                        var candidateId = await _userRepository.GetCandidateId(userId);
                        var vacanciesRec = await _recommendationService.GetRecommendedVacancies(candidateId);
                        var vacanciesIds = vacanciesRec.Select(v => v.VacancyId).ToList();
                        var vacancies = await _repo.GetVacanciesByIds(vacanciesIds);

                        vacancy.PageCount = 1;
                        foreach (var item in vacancies)
                        {
                            var companyName = await _recruiterRepository.GetRecruiterCompanyById(item.RecruiterId);
                            vacancy.Vacancies.Add(new VacancyShortInfo
                            {
                                Company = companyName,
                                Description = item.Description,
                                Id = item.Id,
                                Title = item.Title,
                                Location = item.Location,
                            });
                        }

                    }
                    else
                    {
                        var candidateId = await _userRepository.GetCandidateId(userId);
                        var vacancies = await _repo.GetNewVacancies(candidateId);
                        IEnumerable<Vacancy> filtered = vacancies;

                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            filtered = filtered.Where(v => v.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                        }
                        var filteredList = filtered.ToList();
                        var totalItems = PageCount(filteredList.Count, pageSize);
                        var items = filteredList
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
                        vacancy.PageCount = totalItems;
                        foreach (var item in items)
                        {
                            var companyName = await _recruiterRepository.GetRecruiterCompanyById(item.RecruiterId);
                            vacancy.Vacancies.Add(new VacancyShortInfo
                            {
                                Company = companyName,
                                Description = item.Description,
                                Id = item.Id,
                                Title = item.Title,
                                Location = item.Location,
                            });
                        }
                    }
                    
                }
            }

            return vacancy;
        }

        private int PageCount(int totalItems, int pageSize)
        {
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return totalPages;
        }

        public async Task ApplyVacancy(int vacancyId, int userId)
        {
            var candidateId = await _userRepository.GetCandidateId(userId);

            await _repo.VacancyApply(vacancyId, candidateId);
        }

        public async Task<VacancyLongInfo> GetVacancyInfo(int vacancyId)
        {
            var vacancy = await _repo.GetByIdAsync(vacancyId);
            var company = await _recruiterRepository.GetRecruiterCompanyById(vacancy.RecruiterId);

            return new VacancyLongInfo
            {
                Company = company,
                Description = vacancy.Description,
                Location = vacancy.Location,
                Requirements = vacancy.Requirements,
                SalaryMax = vacancy.SalaryMax,
                SalaryMin = vacancy.SalaryMin,
                Title = vacancy.Title,
                Id = vacancyId
            };
        }

        public async Task<VacancyRecruiterDTO> GetVacancyRecruiter(int vacancyId)
        {
            var vacancy = await _repo.GetByIdAsync(vacancyId);
            var company = await _recruiterRepository.GetRecruiterCompanyById(vacancy.RecruiterId);
            var users = await _repo.GetCandidates(vacancyId);
            var userIds = users.Select(x => x.Id).ToList();
            var Vacancies = new List<CandidateShort>();

            foreach(var user in users)
            {
                string status = await _applicationRepository.GetStatus(user.Id, vacancyId);
                string email = await _userRepository.GetUserEmail(user.UserId);
                var skills = await _tagRepository.GetUserTags(user.UserId);
                Vacancies.Add(new CandidateShort
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Experience = user.Experience,
                    UserId = user.Id,
                    Status = status,
                    Email = email,
                    Skills = skills
                });
            }

            return new VacancyRecruiterDTO
            {
                Company = company,
                Description = vacancy.Description,
                Id = vacancyId,
                Location = vacancy.Location,
                Requirements = vacancy.Requirements,
                SalaryMax = vacancy.SalaryMax,
                SalaryMin = vacancy.SalaryMin,
                Title = vacancy.Title,
                Candidates = Vacancies
            };
        }
    }
}

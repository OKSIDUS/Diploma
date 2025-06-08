using AutoMapper;
using JobVacancies.RecommendationSystem.Services;
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

        public VacancyService(IVacancyRepository repo, IMapper mapper, IRecruiterRepository recruiterRepository, ITagRepository tagRepository, IUserRepository userRepository, RecommendationService recommendationService)
        {
            _repo = repo;
            _mapper = mapper;
            _recruiterRepository = recruiterRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _recommendationService = recommendationService;
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

        public async Task<VacancyPage> GetVacancyPage(int page, int pageSize, bool isRecommendation, int userId)
        {
            VacancyPage vacancy = new VacancyPage();
            vacancy.Page = page;
            vacancy.PageSize = pageSize;
            if (userId > 0)
            {
                var userRole = await _userRepository.GetUserRole(userId);

                if (userRole == "Recruiter")
                {
                    var company = await _recruiterRepository.GetRecruiterCompany(userId);
                    var vacancies = await _repo.GetByRecruiterIdAsync(userId);
                    var totalItems = PageCount(vacancies.Count, pageSize);
                    var items = vacancies
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
                        var vacanciesRec = await _recommendationService.GetRecommendedVacancies(userId);
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
                        var vacancies = await _repo.GetNewVacancies();
                        var totalItems = PageCount(vacancies.Count, pageSize);
                        var items = vacancies
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
    }
}

using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Recruiter;
using JobVacanciesAPI.BAL.DTOs.User;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ICandidateRepository candidateRepository, IRecruiterRepository recruiterRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _candidateRepository = candidateRepository;
            _recruiterRepository = recruiterRepository;
            _mapper = mapper;
        }

        public async Task EditRecruiterProfile(RecruiterEditDTO recruiterEditDTO)
        {
            if(recruiterEditDTO != null)
            {
                await _userRepository.EditUserEmail(recruiterEditDTO.Email, recruiterEditDTO.UserId);


                await _recruiterRepository.UpdateAsync(new DAL.Entity.Recruiter
                {
                    CompanyName = recruiterEditDTO.CompanyName,
                    Position = recruiterEditDTO.Position,
                    UserId = recruiterEditDTO.UserId,
                });
            }
        }

        public async Task<UserProfile> GetUserProfileAsync(int userId)
        {
            if (userId < 1)
            {
                return null;
            }

            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                return null;
            }

            UserProfile profile = new UserProfile
            {
                User = new UserDTO
                {
                    Id = userId,
                    Email = user.Email,
                    Role = user.Role,
                }
            };

            if (user.Role == "Recruiter")
            {
                var recruiter = await _recruiterRepository.GetByIdAsync(userId);
                if(recruiter != null)
                {
                    profile.Recruiter = new DTOs.Recruiter.RecruiterDTO
                    {
                        Id = recruiter.Id,
                        CompanyName = recruiter.CompanyName,
                        Position = recruiter.Position,
                        UserId = userId
                    };
                }
                
            }
            else if (user.Role == "Candidate")
            {
                var candidate = await _candidateRepository.GetByIdAsync(userId);
                if(candidate != null)
                {
                    profile.Candidate = new DTOs.Candidate.CandidateDTO
                    {
                        Id = candidate.Id,
                        DateOfBirth = candidate.DateOfBirth,
                        Experience = candidate.Experience,
                        FullName = candidate.FullName,
                        ResumeFilePath = candidate.ResumeFilePath,
                        Skills = candidate.Skills,
                        UserId = userId
                    };
                }
            }

            return profile;
        }

        async Task IUserService.EditCandidateProfile(CandidateEditDTO candidateEditDTO)
        {
            if(candidateEditDTO != null)
            {
                await _userRepository.EditUserEmail(candidateEditDTO.Email, candidateEditDTO.UserId);

                await _candidateRepository.UpdateAsync(new DAL.Entity.Candidate
                {
                    DateOfBirth = candidateEditDTO.DateOfBirth,
                    Experience = candidateEditDTO.Experience,
                    FullName = candidateEditDTO.FullName,
                    ResumeFilePath = candidateEditDTO.ResumeFilePath,
                    Skills= candidateEditDTO.Skills,
                    UserId = candidateEditDTO.UserId
                });
            }
        }
    }
}

﻿using JobVacanciesAPI.BAL.DTOs.Tag;
using JobVacanciesAPI.BAL.DTOs.User;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPP.BAL.DTOs.UserProfile;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITagService _tagService;

        public UserController(IUserService userService, ITagService tagService)
        {
            _userService = userService;
            _tagService = tagService;
        }


        [HttpGet("get-user-profile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Incorrect user ID: {id}");
            }

            var profile = await _userService.GetUserProfileAsync(id);

            return Ok(profile);

        }

        [HttpPost("edit-recruiter-profile")]
        public async Task<IActionResult> EditRecruiter(RecruiterEditDTO recruiterEdit)
        {
            if (recruiterEdit == null)
            {
                return BadRequest("Model was null");
            }

            await _userService.EditRecruiterProfile(recruiterEdit);

            return Ok();
        }

        [HttpPost("edit-candidate-profile")]
        public async Task<IActionResult> EditCandidate(CandidateEditDTO candidateEdit)
        {
            if (candidateEdit == null)
            {
                return BadRequest("Model was null");
            }

            await _userService.EditCandidateProfile(candidateEdit);

            return Ok();
        }

        [HttpGet("get-user-skills/{id}")]
        public async Task<IActionResult> GetUserSkills(int id)
        {
            var skills = await _tagService.GetUserTags(id);
            return Ok(skills);
        }

        [HttpPost("save-user-skills")]
        public async Task<IActionResult> SaveUserSkills([FromBody]SaveUserSkillsDTO userSkills)
        {
            if (userSkills == null)
            {
                return BadRequest();
            }

            await _tagService.SaveUserSkills(userSkills);

            return Ok();
        }

    }
}

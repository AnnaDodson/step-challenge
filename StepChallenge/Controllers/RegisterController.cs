using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using StepChallenge.DataModels;
using StepChallenge.Services;

namespace StepChallenge.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<RegisterController> _logger;
        private readonly ParticipantService _participantService;
        private readonly TeamService _teamService;

        public RegisterController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterController> logger,
            ParticipantService participantService,
            TeamService teamService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _participantService = participantService;
            _teamService = teamService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegisterUserPostAsync([FromBody] InputModel inputModel)
        {
            if (inputModel == null)
            {
                var err = new Dictionary<string, string>();
                err.Add("error", "Invalid request");
                return new BadRequestObjectResult(err);
            }

            // check it's a valid team and the user doesn't already exist
            var teamExists = await _teamService.TeamExists(inputModel.TeamId);

            if (!teamExists)
            {
                var err = new Dictionary<string, string>();
                err.Add("error", "Team does not exist");
                return new BadRequestObjectResult(err);
            }
            
            var user = new IdentityUser {UserName = inputModel.Email, Email = inputModel.Email};
            var result = await _userManager.CreateAsync(user, inputModel.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            var errors = "";
            foreach (var error in result.Errors)
            {
                errors = errors + " " + error.Description;
            }

            if (!string.IsNullOrEmpty(errors))
            {
                var err = new Dictionary<string, string>();
                err.Add("error", errors);
                return new BadRequestObjectResult(err);
            }

            var newUser = await _participantService.CreateNewParticipant(
                new Participant
                {
                    ParticipantName = inputModel.Name,
                    TeamId = inputModel.TeamId,
                    IdentityUser = user
                }
            );

            var response = new LoginModel
            {
                Success = true,
            };

            return new OkObjectResult(response);
        }

        [Route("get_teams")]
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Team>>> RegisterUserGetTeams()
        {
            var teams = await _teamService.GetAllTeams();
            return teams;
        }
    }
}

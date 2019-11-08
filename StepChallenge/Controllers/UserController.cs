using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StepChallenge.DataModels;
using StepChallenge.Services;

namespace StepChallenge.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterController> _logger;
        private readonly ParticipantService _participantService;
        private readonly StepContext _stepContext;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterController> logger,
            ParticipantService participantService,
            StepContext stepContext
            )
        {
            _userManager = userManager;
           _signInManager = signInManager;
           _roleManager = roleManager;
            _logger = logger;
            _participantService = participantService;
            _stepContext = stepContext;
        }
       
        [HttpPost]
        [Route("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnPostAsync([FromBody] InputModel inputModel)
        {
            var result = new LoginModel();
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var loginResult = await _signInManager.PasswordSignInAsync(inputModel.Username, inputModel.Password, false, lockoutOnFailure: true);
            if (!loginResult.Succeeded)
            {
                _logger.LogInformation($"Failed login attempt for: {inputModel.Username}");
                result.Error = "Incorrect username or password. Try again";
                result.ErrorLogMessage = "Is locked out: " + loginResult.IsLockedOut + ". Is not allowed: " + loginResult.IsNotAllowed + ". Requires Two factor: " + loginResult.RequiresTwoFactor;
                 return new BadRequestObjectResult(result);
            }

            result.Success = true;

            var user = await _userManager.FindByNameAsync(inputModel.Username);
            var adminRole = await _roleManager.FindByNameAsync("Admin");

            if (await _userManager.IsInRoleAsync(user, adminRole.Name))
            {
                result.IsAdmin = true;
            }
            
            _logger.LogInformation($"User Successfully logged in: {inputModel.Username}");

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnPostLogOut(InputModel mode)
        {
            _logger.LogInformation($"User Successfully logged in: {mode.Username}");
            await _signInManager.SignOutAsync();
            return new OkResult();
        }

        [HttpGet]
        [Route("is_admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnGetIsAdmin()
        {
            var response = new LoginModel();
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                response.Error = "No user found";
                return new BadRequestObjectResult(response);
            }

            var adminRole = await _roleManager.FindByNameAsync("Admin");

            if (await _userManager.IsInRoleAsync(user, adminRole.Name))
            {
                response.Success = true;
                response.IsAdmin = true;
            }

            _logger.LogInformation($"Navigating to the admin page {user.UserName}");

            return new OkObjectResult(response);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("edit_user")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnPostEditUser([FromBody] EditUserModel model)
        {
            var user = await _stepContext.Participants
                .Where(u => u.ParticipantId == model.ParticipantId)
                .Include(u => u.IdentityUser)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("No user found");
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (user.IdentityUser.LockoutEnd != null)
                {
                    user.IdentityUser.LockoutEnd = null;
                    await _userManager.UpdateAsync(user.IdentityUser);
                }

                await _userManager.RemovePasswordAsync(user.IdentityUser);
                var passwordResult = await _userManager.AddPasswordAsync(user.IdentityUser, model.Password);

                if (passwordResult.Errors.Any())
                {
                    var err = new Dictionary<string, string> {{"error", passwordResult.Errors.First().Description}};
                    return new BadRequestObjectResult(err);
                }
            }

            if (model.IsAdmin != user.IsAdmin)
            {
                if (model.IsAdmin)
                {
                    await _participantService.AddAdminAccess(user);
                }

                if (!model.IsAdmin)
                {
                    await _participantService.RemoveAdminAccess(user);
                }

                user.IsAdmin = model.IsAdmin;
                await _stepContext.SaveChangesAsync();
            }

            var result = await _userManager.UpdateAsync(user.IdentityUser);
            if (result.Errors.Any())
            {
                var err = new Dictionary<string, string> {{"error", result.Errors.First().Description}};
                return new BadRequestObjectResult(err);
            }

            _logger.LogInformation($"Admin reset {user.ParticipantName} password ");

            var response = new Dictionary<string, string> {{"success", "User updated"}};
            return new OkObjectResult(response);
        }

    }
}
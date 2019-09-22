using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StepChallenge.DataModels;
using StepChallenge.Services;

namespace StepChallenge.Controllers
{
    [Route("api/[controller]")]
    public class UserController
    {
        
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly ILogger<RegisterController> _logger;
        private readonly UserService _userService;
       
        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            //ILogger<RegisterController> logger,
            UserService userService)
        {
            _userManager = userManager;
           _signInManager = signInManager;
            //_logger = logger;
            _userService = userService;
        }
       
        [HttpPost]
        [Route("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnPostAsync([FromBody] InputModel inputModel)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(inputModel.Username, inputModel.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                 var err = new Dictionary<string, string>()
                     {{"error", "Incorrect username or password. Try again"}};
                 return new BadRequestObjectResult(err);
            }
            
            return new OkResult();
        }

        [HttpPost]
        [Route("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnPostLogOut(InputModel mode)
        {
            await _signInManager.SignOutAsync();
            return new OkResult();
        }

    }
}
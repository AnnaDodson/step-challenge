using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepChallenge.Mutation;
using StepChallenge.Services;

namespace StepChallenge.Controllers
{
      [Authorize]
      [Route("api/[controller]")]
      public class StepsController : Controller
      {
         private readonly StepContext _db;
         private readonly UserManager<IdentityUser> _userManager;
         private readonly StepsService _stepsService;

         public StepsController(
            StepContext db,
            UserManager<IdentityUser> userManager,
            StepsService stepsService
         )
         {
            _db = db;
            _userManager = userManager;
            _stepsService = stepsService;
         }
         
         [HttpPost]
         [Route("save_steps")]
         [Consumes(MediaTypeNames.Application.Json)]
         [ProducesResponseType(StatusCodes.Status201Created)]
         [ProducesResponseType(StatusCodes.Status400BadRequest)]
         public async Task<IActionResult> OnPostAsync([FromBody] StepsInputs newSteps)
         {
             var user = await _userManager.GetUserAsync(User);

             var participant = await _db.Participants
                .FirstOrDefaultAsync(p => p.IdentityUser.Id == user.Id);

             if (participant == null)
             {
                return BadRequest("Not a valid participant");
             }

             newSteps.ParticipantId = participant.ParticipantId;
             
             /*
             var savedSteps = await _stepsService.CreateAsync(newSteps);

             if (savedSteps == null)
             {
                return BadRequest("Something went wrong with your update");
             }

                 */
             var result = new Dictionary<string, int>()
                 {{"stepCount", 1}};
             return new OkObjectResult(result);
         }
      }
   }

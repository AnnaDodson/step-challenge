using System.Linq;
using System.Threading.Tasks;
using StepChallenge.Query;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.GraphQL;
using IAuthorizationEvaluator = GraphQL.Authorization.IAuthorizationEvaluator;

namespace StepChallenge.Controllers
{
   [Authorize]
   [Route("graphql")]
   public class GraphQlController : Controller
   {
      private readonly StepContext _db;
      private readonly ISchema _schema;
      private readonly ILogger<RegisterController> _logger;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly IAuthorizationEvaluator _authorizationEvaluator;

      public GraphQlController(
         StepContext db,
         ISchema schema,
         ILogger<RegisterController> logger,
         UserManager<IdentityUser> userManager,
         IHttpContextAccessor httpContextAccessor,
         IAuthorizationEvaluator authorizationEvaluator
         )
      {
         _db = db;
         _schema = schema;
         _logger = logger;
         _userManager = userManager;
         _httpContextAccessor = httpContextAccessor;
         _authorizationEvaluator = authorizationEvaluator;
      }
      
      public async Task<IActionResult> PostGraphQL( [FromBody] GraphQLQuery graphQuery)
      {
         if(graphQuery == null)
         {
            return BadRequest("No Query");
         }
         
         var inputs = graphQuery.Variables.ToInputs();
         var user = await _userManager.GetUserAsync(User);
         if (user == null)
         {
            _logger.LogInformation($"Query failed - no valid user");
            return BadRequest("Not a valid user");
         }
         
         if (inputs.ContainsKey("participantId"))
         {
            var participant = await _db.Participants
               .FirstOrDefaultAsync(p => p.IdentityUser.Id == user.Id);

            if (participant == null)
            {
               _logger.LogInformation($"Query failed - no valid participant {user.UserName}");
               return BadRequest("Not a valid participant");
            }
            
            inputs["participantId"] = participant.ParticipantId.ToString();
         }

         if (inputs.ContainsKey("teamId"))
         {
            var participant = await _db.Participants
               .FirstOrDefaultAsync(p => p.IdentityUser.Id == user.Id);

            if (participant == null)
            {
               _logger.LogInformation($"Query failed - no valid participant {user.UserName}");
               return BadRequest("Not a valid participant");
            }
            inputs["teamId"] = participant.TeamId.ToString();
         }

         var context = _httpContextAccessor.HttpContext;

         var graphQlUserContext = new GraphQLUserContext
         {
            User = context.User
         };

         var result = await new DocumentExecuter().ExecuteAsync(_ =>
         {
            _.Schema = _schema;
            _.Query = graphQuery.Query;
            _.OperationName = graphQuery.OperationName;
            _.Inputs = inputs;
            _.UserContext = graphQlUserContext;
            _.ValidationRules = DocumentValidator.CoreRules().Concat(new IValidationRule[]
            {
               new AuthorizationValidationRule(_authorizationEvaluator)
            });
         }).ConfigureAwait(false);

         if (result.Errors?.Count > 0)
         {
            var msg = "";
            foreach (var err in result.Errors)
            {
               msg = msg + " " + err.Message;
            }
            _logger.LogInformation($"Query failed: {msg}");
            return BadRequest(result.Errors.Select(e => e.Message));
         }

         return Ok(result.Data);
      }

   }
}

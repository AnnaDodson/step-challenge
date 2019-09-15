using System.Linq;
using System.Threading.Tasks;
using StepChallenge.Query;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Mvc;
using StepChallenge.Validation;

namespace StepChallenge.Controllers
{
   [Route("graphql")]
   public class GraphQlController : Controller
   {
      private readonly StepContext _db;
      private readonly ISchema _schema;

      public GraphQlController(StepContext db, ISchema schema)
      {
         _db = db;
         _schema = schema;
      }
      
      public async Task<IActionResult> Post([FromBody] GraphQLQuery graphQuery)
      {
         if(graphQuery == null)
         {
            return BadRequest("No Query");
         }
         
         var inputs = graphQuery.Variables.ToInputs();

         var result = await new DocumentExecuter().ExecuteAsync(_ =>
         {
            _.Schema = _schema;
            _.Query = graphQuery.Query;
            _.OperationName = graphQuery.OperationName;
            _.Inputs = inputs;
            _.ValidationRules = DocumentValidator.CoreRules().Concat(new IValidationRule[]
            {
               new StepValidationRule()
            });
         }).ConfigureAwait(false);

         if (result.Errors?.Count > 0)
         {
            var msg = "";
            foreach (var err in result.Errors)
            {
               msg = msg + " " + err.Message;
            }
            return BadRequest(msg);
         }

         return Ok(result.Data);
      }
      
   }
}

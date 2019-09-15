using GraphQL.Types;
using Model;
using Model.GraphQL;
using StepChallenge.Services;

namespace StepChallenge.Mutation
{
    public class StepChallengeMutation : ObjectGraphType
    {
        public StepChallengeMutation(StepsService stepsService)
        {
            Name = "Mutation";

            Field<StepsType>(
                "creatStepsEntry",
                arguments:  new QueryArguments(
                                new QueryArgument<NonNullGraphType<StepInputType>> {Name = "steps" }),
                resolve: context =>
                {
                    var steps = context.GetArgument<StepsInputs>("steps");
                    var savedSteps = stepsService.CreateAsync(steps);
                    return savedSteps;
                });
        }
    }
}

/*
 *     /// <example>
       /// This is an example JSON request for a mutation
       ///
       /// { "query" : "mutation { creatStepsEntry (steps : {'StepCount' : 444 , 'DateOfSteps' : 2019-09-04T00:00:00+00:00 , 'UserId' : 1 })}" }
       /// 
*/
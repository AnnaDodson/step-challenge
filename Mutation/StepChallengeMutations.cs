using GraphQL.Types;
using Model;
using Model.GraphQL;
using StepChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StepChallenge.DataModels;

namespace StepChallenge.Mutation
{
    public class StepChallengeMutation : ObjectGraphType
    {
        public StepChallengeMutation(StepContext db, StepsService stepsService)
        {
            Name = "Mutation";

            Field<StepsType>(
                "creatStepsEntry",
                arguments:  new QueryArguments(
                                new QueryArgument<NonNullGraphType<StepInputType>> {Name = "steps" }),
                resolve: context =>
                {
                    var steps = context.GetArgument<StepsInputs>("steps");
                    var stepsDay = new DateTime(steps.DateOfSteps.Year, steps.DateOfSteps.Month, steps.DateOfSteps.Day, 0, 0, 0);

                    var existingStepCount = db.Steps
                        .Where(s => s.ParticipantId == steps.ParticipantId)
                        .FirstOrDefault(s => s.DateOfSteps == stepsDay);
                    
                    var savedSteps = new Steps();

                    if(existingStepCount != null){
                         return stepsService.Update(existingStepCount, steps);
                    }
                    else{
                        return stepsService.Create(steps);
                    }
                    
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
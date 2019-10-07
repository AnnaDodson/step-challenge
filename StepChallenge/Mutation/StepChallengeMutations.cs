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

                    if(existingStepCount != null){
                        return stepsService.Update(existingStepCount, steps);
                    }
                    
                    return stepsService.Create(steps);
                });
            
            Field<ChallengeSettingsType>(
                "updateChallengeSettings",
                arguments:  new QueryArguments(
                                new QueryArgument<NonNullGraphType<ChallengeSettingsInputType>> {Name = "settings" }),
                resolve: context =>
                {
                    var newSettings = context.GetArgument<ChallengeSettingsInput>("settings");

                    var existingSettings = db.ChallengeSettings
                        .FirstOrDefault();

                    if (existingSettings != null)
                    {
                        existingSettings.ShowLeaderBoard = newSettings.ShowLeaderBoard;
                        existingSettings.ShowLeaderBoardStepCounts = newSettings.ShowLeaderBoardStepCounts;
                    }

                    db.SaveChanges();
                    
                    return existingSettings;
                });
            
            Field<TeamType>(
                "updateTeam",
                arguments:  new QueryArguments(
                                new QueryArgument<NonNullGraphType<TeamInputType>> {Name = "team" }),
                resolve: context =>
                {
                    var newTeamInfo = context.GetArgument<TeamInput>("team");

                    var team = db.Team
                        .FirstOrDefault(t => t.TeamId == newTeamInfo.TeamId);

                    if (team != null)
                    {
                        team.TeamName = newTeamInfo.TeamName;
                        team.NumberOfParticipants = newTeamInfo.NumberOfParticipants;
                    }
                        
                    db.SaveChanges();
                    return team;
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
using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.GraphQL;
using StepChallenge.DataModels;
using StepChallenge.Services;

namespace StepChallenge.Query
{
    public partial class StepChallengeQuery : ObjectGraphType
    {
        public StepChallengeQuery(StepContext db, StepsService stepsService)
        {
            // TODO these should be set in the db and able to change
            var startDate = new DateTime(2019,09,16, 0,0,0);
            var endDate = new DateTime(2019, 12, 05,0,0,0);

            Field<UserType>(
                "User",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the User"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var user = db.User
                        .Include("Team")
                        .FirstOrDefault(i => i.UserId == id);

                    var steps = db.Steps
                        .Where(s => s.UserId == id)
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < endDate)
                        .OrderBy(s => s.DateOfSteps)
                        .ToList();

                    var days = stepsService.GetAllWeeksSteps(steps);

                    if (user != null)
                    {
                        user.Steps = days;
                    }

                    return user;
                });
            
            Field<ListGraphType<UserType>>(
                "Users",
                resolve: context =>
                {
                    var users = db.User;
                    return users;
                });

            Field<ListGraphType<StepsType>>(
                "TeamSteps",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the Team"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var team = db.Team
                        .Include("Users")
                        .FirstOrDefault(i => i.TeamId == id);

                    var steps = db.Steps
                        .Where(s => team.Users.Any(t => t.UserId == s.UserId))
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < endDate)
                        .ToList();

                    var teamSteps = stepsService.GetAllWeeksSteps(steps);

                    return teamSteps;
                });

            Field<TeamType>(
                "Team",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "teamId", Description = "The ID of the Team"}),
                resolve: context =>
                {
                    var teamId = context.GetArgument<int>("teamId");
                    var team = db.Team
                        .Include("Users")
                        .FirstOrDefault(t => t.TeamId == teamId);

                    return team;
                });

            Field<ListGraphType<TeamType>>(
                "Teams",
                resolve: context =>
                {
                    var teams = db.Team
                        .Include("Users")
                        .Include("Users.Steps");

                    return teams;
                });

            Field<LeaderBoardType>(
                "LeaderBoard",
                resolve: context =>
                {
                    var leaderBoard = new LeaderBoard();

                    var teamSteps = db.Team
                        .Select(t => new TeamScores
                        {
                            TeamId = t.TeamId,
                            TeamName = t.Name,
                            TeamStepCount = t.Users.Sum(u => u.Steps.Sum(s => s.StepCount))
                        }).ToList();

                    leaderBoard.TeamScores = teamSteps;

                    return leaderBoard;
                });


        }
    }
}

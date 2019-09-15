using System;
using System.Linq;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Model.GraphQL;

namespace StepChallenge.Query
{
    public partial class StepChallengeQuery : ObjectGraphType
    {
        public StepChallengeQuery(StepContext db)
        {
            var challengeStartDate = new DateTime(2019,09,16);
            var endOfChallengeDate = new DateTime(2019, 12, 05);
            
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
                        .Where(s => s.DateOfSteps >= challengeStartDate && s.DateOfSteps < endOfChallengeDate)
                        .OrderBy(s => s.DateOfSteps)
                        .ToList();

                    if (user != null)
                    {
                        user.Steps = steps;
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
            
            Field<TeamType>(
                "Team",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the Team"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var team = db.Team
                        .Include("Users")
                        .Include("Users.Steps")
                        .FirstOrDefault(i => i.TeamId == id);
                    return team;
                });

            Field<ListGraphType<TeamType>>(
                "Teams",
                resolve: context =>
                {
                    var teams = db.Team.Include("Team.Users");
                    return teams;
                });
        }
    }
}

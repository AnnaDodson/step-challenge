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

            Field<ParticipantType>(
                "Participant",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "participantId", Description = "The ID of the Participant"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("participantId");
                    var participant = db.Participants
                        .Include("Team")
                        .FirstOrDefault(i => i.ParticipantId == id);

                    var steps = db.Steps
                        .Where(s => s.ParticipantId == id)
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < endDate)
                        .OrderBy(s => s.DateOfSteps)
                        .ToList();

                    if (participant != null)
                    {
                        participant.Steps = steps;
                    }

                    return participant;
                });
            
            Field<ListGraphType<UserType>>(
                "Users",
                resolve: context =>
                {
                    var participants = db.Participants
                        .Include("IdentityUser")
                        .Include("Team");

                    return participants;
                });

            Field<ListGraphType<StepsType>>(
                "TeamSteps",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "teamId", Description = "The ID of the Team"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("teamId");
                    var team = db.Team
                        .Include("Participants")
                        .FirstOrDefault(i => i.TeamId == id);

                    var teamSteps = db.Steps
                        .Where(s => team.Participants.Any(t => t.ParticipantId == s.ParticipantId))
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < endDate)
                        .GroupBy(s => s.DateOfSteps)
                        .Select(s => new Steps
                        {
                            DateOfSteps = s.First().DateOfSteps,
                            StepCount = s.Sum(st => st.StepCount)
                        })
                        .OrderBy(s => s.DateOfSteps)
                        .ToList();

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
                        .Include("Participants")
                        .FirstOrDefault(t => t.TeamId == teamId);

                    return team;
                });

            Field<ListGraphType<TeamType>>(
                "Teams",
                resolve: context =>
                {
                    var teams = db.Team
                        .Include("Participants")
                        .Include("Participants.Steps");

                    return teams;
                });

            Field<LeaderBoardType>(
                "LeaderBoard",
                resolve: context =>
                {
                    var leaderBoard = new LeaderBoard();

                    DateTime thisWeek = DateTime.Now;
                    DateTime thisMonday = stepsService.StartOfWeek(thisWeek, DayOfWeek.Monday);

                    leaderBoard.DateOfLeaderboard = thisMonday;

                    var sortedTeams = db.Team
                        .Where(t => t.Participants.Any(p => p.Steps.Any(s => s.DateOfSteps > startDate && s.DateOfSteps < thisMonday ))
                            || t.Participants.All(p => p.Steps.All(s => s.StepCount == 0)))
                        .OrderByDescending(t => t.Participants.Sum(u => u.Steps.Sum(s => s.StepCount)))
                        .ToList();

                    var teamSteps = sortedTeams.Select(t => new TeamScores
                    {
                        TeamId = t.TeamId,
                        TeamName = t.TeamName
                    }).ToList();

                    leaderBoard.TeamScores = teamSteps;

                    return leaderBoard;
                });


        }
    }
}

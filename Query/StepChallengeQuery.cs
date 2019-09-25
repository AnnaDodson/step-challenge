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

                    var days = stepsService.GetLatestWeeksSteps(steps);

                    if (participant != null)
                    {
                        participant.Steps = days;
                    }

                    return participant;
                });
            
            Field<ListGraphType<ParticipantType>>(
                "Participants",
                resolve: context =>
                {
                    var participants = db.Participants;
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

                    var steps = db.Steps
                        .Where(s => team.Participants.Any(t => t.ParticipantId == s.ParticipantId))
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < endDate)
                        .ToList();

                    var teamSteps = stepsService.GetLatestWeeksSteps(steps);

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

                    var teamSteps = db.Team
                        .Select(t => new TeamScores
                        {
                            TeamId = t.TeamId,
                            TeamName = t.TeamName,
                            TeamStepCount = t.Participants.Sum(u => u.Steps.Sum(s => s.StepCount))
                        }).ToList();

                    leaderBoard.TeamScores = teamSteps;

                    return leaderBoard;
                });


        }
    }
}

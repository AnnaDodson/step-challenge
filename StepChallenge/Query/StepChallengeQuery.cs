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
        public StepChallengeQuery(StepContext db, StepsService stepsService, TeamService teamService)
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

                    return stepsService.GetParticipantSteps(participant);
                });
            
            Field<ListGraphType<UserType>>(
                "Users",
                resolve: context =>
                {
                    var participants = db.Participants
                        .Include("IdentityUser")
                        .Include("Team")
                        .OrderBy(u => u.ParticipantName);

                    return participants;
                });

            Field<ListGraphType<TeamScoreBoardType>>(
                "TeamSteps",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "teamId", Description = "The ID of the Team"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("teamId");
                    var teamSteps = teamService.GetTeamScoreBoard(id);

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
                    var settings = db.ChallengeSettings
                        .FirstOrDefault();

                    if (settings == null || !settings.ShowLeaderBoard)
                    {
                        leaderBoard.TeamScores = new List<TeamScores>();
                        return leaderBoard;
                    }

                    var teams = db.Team;
                    leaderBoard = stepsService.GetLeaderBoard(teams);

                    var teamSteps = leaderBoard.TeamScores.Select(t => new TeamScores
                    {
                        TeamId = t.TeamId,
                        TeamName = t.TeamName,
                        TeamStepCount = settings.ShowLeaderBoardStepCounts ? t.TeamStepCount : 0
                    }).ToList();

                    leaderBoard.TeamScores = teamSteps;

                    return leaderBoard;
                });

            Field<AdminLeaderBoardType>(
                "AdminLeaderBoard",
                resolve: context =>
                {
                    var teams = db.Team;

                    var leaderBoard = stepsService.GetLeaderBoard(teams);
                
                    return leaderBoard;
                });

            Field<ChallengeSettingsType>(
                "ChallengeSettings",
                resolve: context =>
                {
                    var settings = db.ChallengeSettings
                        .FirstOrDefault();

                    return settings;
                });


        }

    }
}

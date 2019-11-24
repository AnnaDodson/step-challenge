using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.GraphQL;
using StepChallenge.DataModels;
using StepChallenge.Mutation;

namespace StepChallenge.Services
{
    public class StepsService
    {
        public StepContext _stepContext;
        public DateTime _startDate = new DateTime(2019,09,16, 0,0,0);
        public DateTime _endDate = new DateTime(2019, 12, 05,0,0,0);

        public StepsService(StepContext stepContext)
        {
            _stepContext = stepContext;
        }

        public Steps Update(Steps existingStepCount, StepsInputs steps)
        {
            existingStepCount.StepCount = steps.StepCount;

            _stepContext.SaveChanges();

            return existingStepCount;
        }

        public Steps Create(StepsInputs steps)
        {
            var challengeStartDate = new DateTime(2019,09,16);
            var currentCulture = CultureInfo.CurrentCulture;

            var challengeStartWeek = currentCulture.Calendar.GetWeekOfYear(
                new DateTime(challengeStartDate.Year, challengeStartDate.Month, challengeStartDate.Day),
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);

            var stepsWeekNo = currentCulture.Calendar.GetWeekOfYear(
                new DateTime(steps.DateOfSteps.Year, steps.DateOfSteps.Month, steps.DateOfSteps.Day),
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);

            var weekNo = (stepsWeekNo - challengeStartWeek) + 1;

            var stepsDay = new DateTime(steps.DateOfSteps.Year, steps.DateOfSteps.Month, steps.DateOfSteps.Day, 0,
                0, 0);

            var newSteps = new Steps
            {
                DateOfSteps = stepsDay,
                StepCount = steps.StepCount,
                Day = (int)steps.DateOfSteps.DayOfWeek,
                ParticipantId = steps.ParticipantId,
                Week = weekNo,
            };

            _stepContext.Steps.Add(newSteps);
            _stepContext.SaveChanges();

            return newSteps;
        }

        public LeaderBoard GetLeaderBoard(IQueryable<Team> teams)
        {
            var thisMonday = StartOfWeek(DateTime.Now, DayOfWeek.Monday);

            var leaderBoard = new LeaderBoard
            {
                DateOfLeaderboard = thisMonday,
                TeamScores = GetTeamScores(teams, thisMonday, _startDate, GetTeamSize()),
                TotalSteps = GetTotalSteps(teams, thisMonday, _startDate, GetTeamSize())
            };

            return leaderBoard;
        }

        public Participant GetParticipantSteps(Participant participant)
        {
            var steps = _stepContext.Steps
                .Where(s => s.ParticipantId == participant.ParticipantId)
                .Where(s => s.DateOfSteps >= _startDate && s.DateOfSteps < _endDate)
                .OrderBy(s => s.DateOfSteps)
                .ToList();

            participant.Steps = steps;

            return participant;
        }

        public List<TeamScores> GetTeamScores(IQueryable<Team> teams, DateTime thisMonday, DateTime startDate, int averageTeamSize)
        {
            var sortedTeams = teams
                .Select(t => new TeamScores
                {
                    TeamId = t.TeamId,
                    TeamName = t.TeamName,
                    NumberOfParticipants = t.NumberOfParticipants,
                    TeamStepCount = t.Participants.Sum(p => p.Steps
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < thisMonday
                                    || t.Participants.All(u => u.Steps.All(x => x.StepCount == 0)))
                        .Sum(s => s.StepCount))
                })
                .ToList();

            var teamWithAveragePerson = GetAverageStepsForTeamWithLessPeople(sortedTeams, averageTeamSize);

            return teamWithAveragePerson.OrderByDescending(t => t.TeamStepCount).ToList();
        }

        private List<TeamScores> GetAverageStepsForTeamWithLessPeople(List<TeamScores> teams, int averageTeamSize)
        {
            // add a pretend participant with averaged steps to teams with less participants
            foreach (var teamScore in teams.Where(t => t.NumberOfParticipants != averageTeamSize && t.NumberOfParticipants != 0))
            {
                teamScore.TeamStepCount = GetAveragedStepCountTotal(teamScore.TeamStepCount,
                    teamScore.NumberOfParticipants, averageTeamSize);
            }
            return teams;
        }

        private List<TeamScoresOverview> GetAverageStepsForTeamWithLessPeople(List<TeamScoresOverview> teams, int averageTeamSize)
        {
            // add a pretend participant with averaged steps to teams with less participants
            foreach (var teamScore in teams.Where(t => t.NumberOfParticipants != averageTeamSize && t.NumberOfParticipants != 0))
            {
                teamScore.TeamTotalStepsWithAverage = GetAveragedStepCountTotal(teamScore.TeamTotalSteps, teamScore.NumberOfParticipants, averageTeamSize);
            }
            return teams;
        }

        private int GetAveragedStepCountTotal(int teamStepCount, int teamNumberOfParticipants, int averageTeamSize)
        {
            return ((teamStepCount / teamNumberOfParticipants) *
                                           (averageTeamSize - teamNumberOfParticipants)) +
                                          teamStepCount;
        }

        public int GetTotalSteps(IQueryable<Team> teams, DateTime thisMonday, DateTime startDate, int averageTeamSize)
        {
            var total = teams
                .Sum(t => t.Participants.Sum(p => p.Steps
                        .Where(s => s.DateOfSteps >= startDate && s.DateOfSteps < thisMonday
                                    || t.Participants.All(u => u.Steps.All(x => x.StepCount == 0)))
                        .Sum(s => s.StepCount)));

            return total;
        }

        public async Task<AdminParticipantsOverview> GetTeamsOverview()
        {
            var overview = new AdminParticipantsOverview
            {
                HighestStepsParticipant = 0,
                HighestStepsParticipantId = 0,
                HighestStepsTeam = 0,
                HighestStepsTeamId = 0,
            };

            var teams = _stepContext.Team
                .Select(team => new TeamScoresOverview
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    NumberOfParticipants = team.NumberOfParticipants,
                    TeamTotalSteps = team.Participants.Sum(p => p.Steps
                        .Where(s => s.DateOfSteps >= _startDate && s.DateOfSteps < _endDate)
                        .Sum(s => s.StepCount)),
                    ParticipantsStepsOverviews = team.Participants.Select(participant => new ParticipantsStepsOverview
                    {
                        ParticipantId = participant.ParticipantId,
                        ParticipantName = participant.ParticipantName,
                        StepsOverviews = GetAllDaysSteps(participant.Steps),
                        StepTotal = participant.Steps.Where(s => s.DateOfSteps >= _startDate && s.DateOfSteps < _endDate).Sum(s => s.StepCount)
                    }).ToList()
                })
                .OrderBy(t => t.TeamName)
                .ToList();

            var averageTeamSize = await GetAverageTeamSize();

            //TODO this should be returning all in one flat list so these can be added inline
            //var teamsWithAverageScores = GetAverageStepsForTeamWithLessPeople(teams, averageTeamSize);
            foreach (var team in teams)
            {
                foreach (var participant in team.ParticipantsStepsOverviews)
                {
                    if (participant.StepTotal > overview.HighestStepsParticipant)
                    {
                        overview.HighestStepsParticipant = participant.StepTotal;
                        overview.HighestStepsParticipantId = participant.ParticipantId;
                    }
                }

                if (team.TeamTotalSteps > overview.HighestStepsTeam)
                {
                    overview.HighestStepsTeam = team.TeamTotalSteps;
                    overview.HighestStepsTeamId = team.TeamId;
                }

                if (team.NumberOfParticipants != averageTeamSize && team.NumberOfParticipants != 0)
                {
                    team.TeamTotalStepsWithAverage = GetAveragedStepCountTotal(team.TeamTotalSteps, team.NumberOfParticipants, averageTeamSize);
                }
                if(team.NumberOfParticipants != team.ParticipantsStepsOverviews.Count)
                {
                    team.ParticipantsStepsOverviews.AddRange(GetMissingParticipants(team));
                }
            }

            overview.Teams = teams;

            return overview;
        }

        private List<ParticipantsStepsOverview> GetMissingParticipants(TeamScoresOverview team)
        {
            var need = team.NumberOfParticipants - team.ParticipantsStepsOverviews.Count;
            var emptyParticipant = new ParticipantsStepsOverview
            {
                ParticipantName = "Not registered",
                StepsOverviews = new List<StepsOverview>(),
                StepTotal = 0
            };
            var emptyParticipants = new ParticipantsStepsOverview[need];
            Array.Fill(emptyParticipants, emptyParticipant);

            return emptyParticipants.ToList();
        }

        private List<StepsOverview> GetAllDaysSteps(IEnumerable<Steps> steps)
        {
            var days = new List<StepsOverview>();
            for (var dt = _startDate; dt <= _endDate; dt = dt.AddDays(1))
            {
                var dayStepCount = new StepsOverview
                {
                    StepCount = steps.Any(s => s.DateOfSteps == dt) ? steps.First(s => s.DateOfSteps == dt).StepCount : 0,
                    DateOfSteps = dt,
                };
                days.Add(dayStepCount);
            }

            return days;
        }

        private int GetTeamSize()
        {
            // average number of people in a team
            // this should be saved in the db as part of the challenge set up data
            return 6;
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public virtual async Task<int> GetAverageTeamSize()
        {
            var average = await _stepContext.ChallengeSettings
                .Where(s => s.ChallengeSettingsId == 1)
                .FirstOrDefaultAsync();

            return average?.NumberOfParticipantsInATeam ?? 6;
        }

    }
}
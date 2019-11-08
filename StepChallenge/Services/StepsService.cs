using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
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

            // add a pretend participant with averaged steps to teams with less participants
            foreach (var teamScore in sortedTeams.Where(t => t.NumberOfParticipants != averageTeamSize && t.NumberOfParticipants != 0))
            {
                teamScore.TeamStepCount = ((teamScore.TeamStepCount / teamScore.NumberOfParticipants) *
                                           (averageTeamSize - teamScore.NumberOfParticipants)) +
                                          teamScore.TeamStepCount;
            }

            return sortedTeams.OrderByDescending(t => t.TeamStepCount).ToList();
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

    }
}
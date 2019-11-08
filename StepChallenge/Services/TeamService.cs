using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using StepChallenge.DataModels;

namespace StepChallenge.Services
{
    public class TeamService
    {
        private readonly StepContext _stepContext;
        private DateTime _startDate = new DateTime(2019,09,16, 0,0,0);
        private DateTime _endDate = new DateTime(2019, 12, 05,0,0,0);
        
        public TeamService(StepContext stepContext)
        {
            _stepContext = stepContext;
        }
        
        public async Task<bool> TeamExists(int teamId)
        {
            var team = await _stepContext.Team
                .AnyAsync(t => t.TeamId == teamId);

            return team;
        }
        
        public async Task<List<Team>> GetAllTeams()
        {
            var teams = await _stepContext.Team
                .ToListAsync();

            return teams;
        }

        public List<TeamScoreBoard> GetTeamScoreBoard(int teamId)
        {
            var participants = _stepContext.Participants
                .Where(i => i.TeamId == teamId);

            var teamSteps = _stepContext.Steps
                .Where(s => participants.Any(t => t.ParticipantId == s.ParticipantId))
                .GroupBy(s => s.DateOfSteps)
                .Select(s => new TeamScoreBoard
                {
                    DateOfSteps = s.First().DateOfSteps,
                    StepCount = s.Where(st => st.DateOfSteps >= _startDate && st.DateOfSteps < _endDate)
                        .Sum(st => st.StepCount),
                })
                .OrderBy(s => s.DateOfSteps)
                .ToList();

            foreach (var teamStep in teamSteps)
            {
                var stepsStatuses = participants
                    .Select(p => new ParticipantsStepsStatus
                    {
                        ParticipantName = p.ParticipantName,
                        ParticipantId = p.ParticipantId,
                        ParticipantAddedStepCount =
                            p.Steps.Any(ps => ps.DateOfSteps == teamStep.DateOfSteps && ps.StepCount != 0),
                        ParticipantStepCount = p.Steps.Where(ps => ps.DateOfSteps == teamStep.DateOfSteps).Sum(ps => ps.StepCount),
                    })
                    .OrderByDescending(p => p.ParticipantStepCount)
                    .ToList();

                var highest = stepsStatuses.First().ParticipantStepCount;
                if (highest != 0)
                {
                    foreach (var stepStatus in stepsStatuses.Where(p => p.ParticipantStepCount == highest))
                    {
                        stepStatus.ParticipantHighestStepper = true;
                    }
                }

                teamStep.ParticipantsStepsStatus = stepsStatuses
                    .OrderBy(p => p.ParticipantName)
                    .ToList();
            }

            return teamSteps;
        }
        
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

namespace StepChallenge.Services
{
    public class TeamService
    {
        public StepContext _stepContext;
        
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
        
    }
}
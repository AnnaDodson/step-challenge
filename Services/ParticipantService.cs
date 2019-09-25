using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

namespace StepChallenge.Services
{
    public class UserService
    {
        public StepContext _stepContext;
        
        public UserService(StepContext stepContext)
        {
            _stepContext = stepContext;
        }
        
        public async Task<Participant> CreateNewParticipant(Participant newParticipant)
        {
            var team = await _stepContext.Team
                .FirstOrDefaultAsync(t => t.TeamId == newParticipant.TeamId);

            if (team == null)
            {
                return null;
            }

            newParticipant.Team = team;
            
            _stepContext.Participants.Add(newParticipant);
            await _stepContext.SaveChangesAsync();

            return newParticipant;
        }
    }
}
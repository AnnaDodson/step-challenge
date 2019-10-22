using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using StepChallenge.Controllers;

namespace StepChallenge.Services
{
    public class ParticipantService
    {
        private readonly StepContext _stepContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterController> _logger;

        public ParticipantService(
            StepContext stepContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterController> logger
            )
        {
            _stepContext = stepContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
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

        public async Task AddAdminAccess(Participant participant)
        {
             await _userManager.AddClaimAsync(participant.IdentityUser, new Claim("role", "Admin"));
             var adminRole = await _roleManager.FindByNameAsync("Admin");
             if (adminRole == null)
             {
                 adminRole = new IdentityRole("Admin");
                 await _roleManager.CreateAsync(adminRole);
                 await _roleManager.AddClaimAsync(adminRole, new Claim("Authentication", "Admin"));
             }

             if (!await _userManager.IsInRoleAsync(participant.IdentityUser, adminRole.Name))
             {
                 await _userManager.AddToRoleAsync(participant.IdentityUser, adminRole.Name);
             }
        }

        public async Task RemoveAdminAccess(Participant participant)
        {
             await _userManager.RemoveClaimAsync(participant.IdentityUser, new Claim("role", "Admin"));
             var adminRole = await _roleManager.FindByNameAsync("Admin");
             if (adminRole != null && await _userManager.IsInRoleAsync(participant.IdentityUser, adminRole.Name))
             {
                 await _userManager.RemoveFromRoleAsync(participant.IdentityUser, adminRole.Name);
             }
        }

        public async Task DeleteParticipant(Participant participant)
        {
            var steps = _stepContext.Steps
                .Where(s => s.ParticipantId == participant.ParticipantId);

            _stepContext.Steps.RemoveRange(steps);
            _stepContext.Participants.Remove(participant);
            await _stepContext.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(participant.IdentityUser);

            if (result.Errors.Any())
            {
                var err = new Dictionary<string, string> {{"error", result.Errors.First().Description}};
            }
            _logger.LogInformation($"Admin deleted participant: {participant.ParticipantName}");
        }
    }
}
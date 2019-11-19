using System.Collections.Generic;

namespace StepChallenge.DataModels
{
    public class TeamScoresOverview 
    {
        public List<ParticipantsStepsOverview> ParticipantsStepsOverviews { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int NumberOfParticipants { get; set; }

        public int TeamTotalSteps { get; set; }
        
    }
}
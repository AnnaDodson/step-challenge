using System.Collections.Generic;
using Model;

namespace StepChallenge.DataModels
{
    public class AdminParticipantsOverview
    {
        public List<TeamScoresOverview> Teams { get; set; }
        public int HighestStepsParticipant { get; set; }
        public int HighestStepsParticipantId { get; set; }
        public int HighestStepsTeam { get; set; }
        public int HighestStepsTeamId { get; set; }
    }
}
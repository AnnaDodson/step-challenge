using System;

namespace Model
{
    public class ChallengeSettings
    {
         public int ChallengeSettingsId { get; set; }
         public string Name { get; set; }
         public DateTimeOffset StartDate { get; set; }
         public DateTimeOffset EndDate { get; set; }
         public int DurationInWeeks { get; set; }
         public bool ShowLeaderBoard { get; set; }
         public bool ShowLeaderBoardStepCounts { get; set; }
         public int NumberOfParticipants { get; set; }
         public int NumberOfParticipantsInATeam { get; set; }
        
    }
}
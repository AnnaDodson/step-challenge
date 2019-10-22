using System;
using System.Collections.Generic;
using GraphQL.Types;
using Model;

namespace StepChallenge.DataModels
{
    public class LeaderBoard
    {
        public List<TeamScores> TeamScores { get; set; }
        public DateTimeOffset DateOfLeaderboard { get; set; }
        public int TotalSteps { get; set; }
    }
}
using System.Collections.Generic;
using GraphQL.Types;
using Model;

namespace StepChallenge.DataModels
{
    public class LeaderBoard
    {
        public List<TeamScores> TeamScores { get; set; }
        
    }
}
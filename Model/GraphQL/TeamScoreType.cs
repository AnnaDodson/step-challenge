using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class TeamScoreType : ObjectGraphType<TeamScores>
    {
        public TeamScoreType()
        {
            Field(x => x.TeamId, type: typeof(IdGraphType)).Description("The ID of the Team.");
            Field(x => x.TeamName, type:  typeof(StringGraphType)).Description("The name of the Team");
            Field(x => x.TeamStepCount, type: typeof(IntGraphType)).Description("The total team steps");
        }
        
    }
}
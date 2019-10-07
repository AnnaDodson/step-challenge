using GraphQL.Authorization;
using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class LeaderBoardType : ObjectGraphType<LeaderBoard>
    {
        public LeaderBoardType()
        {
            Name = "LeaderBoard";
            
            Field(x => x.TeamScores , type: typeof(ListGraphType<TeamScoreType>)).Description("The users steps");
            Field(x => x.DateOfLeaderboard, type: typeof(DateTimeOffsetGraphType))
                .Description("The date the scores are taken until");

        }
    }

    public class AdminLeaderBoardType : LeaderBoardType
    {
        public AdminLeaderBoardType()
        {
            Name = "AdminLeaderBoard";

            this.AuthorizeWith("AdminPolicy");
        }
    }
}
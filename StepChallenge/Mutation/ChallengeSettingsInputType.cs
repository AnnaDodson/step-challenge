using GraphQL.Authorization;
using GraphQL.Types;

namespace StepChallenge.Mutation
{
    public class ChallengeSettingsInputType :InputObjectGraphType
    {

        public ChallengeSettingsInputType()
        {
            Name = "ChallengeSettingsInput";

            this.AuthorizeWith("AdminPolicy");
            Field<NonNullGraphType<BooleanGraphType>>("showLeaderBoard").AuthorizeWith("AdminPolicy");
            Field<NonNullGraphType<BooleanGraphType>>("ShowLeaderBoardStepCounts").AuthorizeWith("AdminPolicy");
        }

    }
}
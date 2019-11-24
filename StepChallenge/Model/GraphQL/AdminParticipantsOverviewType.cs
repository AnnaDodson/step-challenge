using GraphQL.Authorization;
using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class AdminParticipantsOverviewType : ObjectGraphType<AdminParticipantsOverview>
    {
        public AdminParticipantsOverviewType()
        {
            Name = "AdminParticipantsOverview";
            this.AuthorizeWith("AdminPolicy");
            
            Field(x => x.Teams , type: typeof(ListGraphType<TeamOverviewType>)).Description("All the Teams");
            Field(x => x.HighestStepsTeam, type: typeof(IntGraphType)).Description("Team with the highest step count");
            Field(x => x.HighestStepsTeamId, type: typeof(IdGraphType)).Description("Id of the Team with the highest step count");
            Field(x => x.HighestStepsParticipant, type: typeof(IntGraphType)).Description("Participant with the highest step count");
            Field(x => x.HighestStepsParticipantId, type: typeof(IntGraphType)).Description("Id of the Participant with the highest step count");
        }
    }
}
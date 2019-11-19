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
            
            Field(x => x.Teams , type: typeof(ListGraphType<TeamType>)).Description("All the Teams");
        }
    }
}
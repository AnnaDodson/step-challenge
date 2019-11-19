using GraphQL.Authorization;
using GraphQL.Types;

namespace Model.GraphQL
{
    public class TeamType : ObjectGraphType<Team>
    {

        public TeamType()
        {
            Name = "Team";
            
            Field(x => x.TeamId, type: typeof(IdGraphType)).Description("The ID of the Team.");
            Field(x => x.TeamName).Description("The name of the Team");
            Field(x => x.NumberOfParticipants).Description("How many Participants are in this team. Includes registered and unregistered users").AuthorizeWith("AdminPolicy");
            Field(x => x.Participants, type: typeof(ListGraphType<ParticipantType>)).Description("Users in the Team").AuthorizeWith("AdminPolicy");
        }
        
    }
}
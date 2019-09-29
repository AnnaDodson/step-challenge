using GraphQL.Authorization;
using GraphQL.Types;

namespace Model.GraphQL
{
    public class UserType : ObjectGraphType<Participant>
    {
        public UserType()
        {
            Name = "UserType";
            
            this.AuthorizeWith("AdminPolicy");
            Field(x => x.ParticipantId, type: typeof(IdGraphType)).Description("The ID of the participant.");
            Field(x => x.IsAdmin, type: typeof(BooleanGraphType)).Description("If the user is admin");
            Field(x => x.TeamId, type: typeof(IdGraphType)).Description("The Team Id");
            Field(x => x.ParticipantName).Description("The name of the participant");
            Field(x => x.Team, type: typeof(TeamType)).Description("The name of the participants team");
            Field(x => x.IdentityUser.Email).Description("Participants Email address");
        }
    }
}
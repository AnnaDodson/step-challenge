using System.Linq;
using GraphiQl;
using GraphQL.Types;

namespace Model.GraphQL
{
    public class ParticipantType : ObjectGraphType<Participant>
    {
        public ParticipantType()
        {
            Name = "Participant";

            Field(x => x.ParticipantId, type: typeof(IdGraphType)).Description("The ID of the participant.");
            Field(x => x.IsAdmin, type: typeof(BooleanGraphType)).Description("If the user is admin");
            Field(x => x.TeamId, type: typeof(IdGraphType)).Description("The Team Id");
            Field(x => x.ParticipantName).Description("The name of the participant");
            Field(x => x.Team, type: typeof(TeamType)).Description("The name of the participants team");
            Field(x => x.Steps, type: typeof(ListGraphType<StepsType>)).Description("The participants steps");
            //Field(x => x.Users, type: typeof(ListGraphType<UserType>)).Description("Users in the Team");
        }
    }
}
using GraphiQl;
using GraphQL.Types;

namespace Model.GraphQL
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Name = "User";

            Field(x => x.UserId, type: typeof(IdGraphType)).Description("The ID of the user.");
            Field(x => x.IsAdmin, type: typeof(BooleanGraphType)).Description("If the user is admin");
            Field(x => x.TeamId, type: typeof(IdGraphType)).Description("The Team Id");
            Field(x => x.UserName).Description("The name of the user");
            Field(x => x.Team, type: typeof(TeamType)).Description("The name of the users team");
            Field(x => x.Steps, type: typeof(ListGraphType<StepsType>)).Description("The users steps");
            //Field(x => x.Users, type: typeof(ListGraphType<UserType>)).Description("Users in the Team");
        }
    }
}
using GraphQL.Authorization;
using GraphQL.Types;

namespace StepChallenge.Mutation
{
    public class TeamInputType : InputObjectGraphType
    {
        public TeamInputType()
        {
            Name = "UpdateTeam";

            this.AuthorizeWith("AdminPolicy");
            Field<NonNullGraphType<IntGraphType>>("NumberOfParticipants").AuthorizeWith("AdminPolicy");
            Field<NonNullGraphType<StringGraphType>>("TeamName").AuthorizeWith("AdminPolicy");
            Field<NonNullGraphType<IdGraphType>>("TeamId").AuthorizeWith("AdminPolicy");
        }
        
    }
}
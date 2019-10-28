using GraphQL.Authorization;
using GraphQL.Types;

namespace StepChallenge.Mutation
{
    public class ParticipantInputType :InputObjectGraphType
    {
        public ParticipantInputType()
        {
            Name = "ParticipantInput";
            Field<NonNullGraphType<IntGraphType>>("participantId").AuthorizeWith("AdminPolicy");;
        }
    }
}

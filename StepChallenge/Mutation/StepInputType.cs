using GraphQL.Types;
using Model;
using StepChallenge.Validation;

namespace StepChallenge.Mutation
{
    public class StepInputType :InputObjectGraphType
    {
        public StepInputType()
        {
            Name = "StepInput";
            Field<NonNullGraphType<IntGraphType>>("stepCount");
            Field<NonNullGraphType<IntGraphType>>("participantId");
            Field<NonNullGraphType<DateTimeGraphType>>("dateOfSteps").Metadata.Add(nameof(StepValidationRule), null);
        }
    }
}

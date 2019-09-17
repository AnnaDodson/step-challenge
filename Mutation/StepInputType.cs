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
            //Field(x => x.StepCount);
            //Field(x => x.UserId);
            //Field(x => x.DateOfSteps);
            Field<NonNullGraphType<IntGraphType>>("stepCount");
            Field<NonNullGraphType<IntGraphType>>("userId");
            Field<NonNullGraphType<DateTimeGraphType>>("dateOfSteps").Metadata.Add(nameof(StepValidationRule), null);
            /*
            Field<NonNullGraphType<DateTimeOffsetGraphType>>(x => x.DateOfSteps)
            .Description("Date the steps were taken")
            .Configure(type => type.Metadata.Add(nameof(StepValidationRule), null));
            */
        }
    }
}

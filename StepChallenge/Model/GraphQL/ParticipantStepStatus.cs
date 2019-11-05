using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class ParticipantStepStatusType : ObjectGraphType<ParticipantsStepsStatus>
    {

        public ParticipantStepStatusType()
        {
            Name = "ParticipantStepStatus";

            Field(x => x.ParticipantName, type: typeof(StringGraphType)).Description("The Date of the step count");
            Field(x => x.ParticipantAddedStepCount, type: typeof(BooleanGraphType)).Description("If the participant has filled in their steps or not");
            Field(x => x.ParticipantHighestStepper, type: typeof(BooleanGraphType)).Description("Participants with the highest step count will be set to true");
        }

    }
}

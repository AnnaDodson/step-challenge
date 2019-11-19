using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class ParticipantOverviewType : ObjectGraphType<ParticipantsStepsOverview>
    {
        public ParticipantOverviewType()
        {
            Name = "ParticipantOverviewType";
            Field(x => x.ParticipantId, type: typeof(IdGraphType)).Description("The ID of the participant.");
            Field(x => x.ParticipantName).Description("The name of the participant");
            Field(x => x.StepTotal, type: typeof(IntGraphType)).Description("The step count total");
            Field(x => x.StepsOverviews, type: typeof(ListGraphType<StepsOverviewType>)).Description("The participants steps");
            //Field(x => x.Users, type: typeof(ListGraphType<UserType>)).Description("Users in the Team");
        }
    }

    public class StepsOverviewType : ObjectGraphType<StepsOverview>
    {
        public StepsOverviewType()
        {
            Name = "StepsOverview";
            Field(x => x.StepCount, type: typeof(IntGraphType)).Description("The step count for this day");
            Field(x => x.DateOfSteps, type: typeof(DateTimeOffsetGraphType)).Description("The date the steps were taken");
        }
    }
}
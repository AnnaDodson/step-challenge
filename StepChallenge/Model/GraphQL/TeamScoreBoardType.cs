using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class TeamScoreBoardType : ObjectGraphType<TeamScoreBoard>
    {

        public TeamScoreBoardType()
        {
            Name = "TeamScoreBoard";

            Field(x => x.DateOfSteps, type: typeof(DateTimeGraphType)).Description("The Date of the step count");
            Field(x => x.StepCount, type: typeof(IntGraphType)).Description("The step count for this day");
            Field(x => x.ParticipantsStepsStatus, type: typeof(ListGraphType<ParticipantStepStatusType>)).Description("Users in the Team and if they have filled in their steps or not");
        }
    }
}
using GraphQL.Types;

namespace Model.GraphQL
{
    public class StepsType : ObjectGraphType<Steps>
    {
        public StepsType()
        {
            Name = "Steps";
            
            Field(x => x.StepsId, type: typeof(IdGraphType)).Description("The ID of the steps.");
            Field(x => x.StepCount, type: typeof(IntGraphType)).Description("The step count for this day");
            Field(x => x.DateOfSteps, type: typeof(DateTimeOffsetGraphType)).Description("The date the steps were taken");
            Field(x => x.Week, type: typeof(IntGraphType)).Description("The week number the steps were taken are in");
            Field(x => x.Day, type: typeof(IntGraphType)).Description("The day of the week the steps were taken");
        }
    }
}


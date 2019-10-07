using GraphQL.Types;

namespace Model.GraphQL
{
    public class ChallengeSettingsType : ObjectGraphType<ChallengeSettings>
    {
        public ChallengeSettingsType()
        {
            Name = "ChallengeSettings";
            
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name of the Challenge");
            Field(x => x.StartDate, type: typeof(DateTimeOffsetGraphType)).Description("The date the challenge starts");
            Field(x => x.EndDate, type: typeof(DateTimeOffsetGraphType)).Description("The date the challenge ends");
            Field(x => x.DurationInWeeks, type: typeof(IntGraphType)).Description("How long the challenge lasts in weeks");
            Field(x => x.ShowLeaderBoard, type: typeof(BooleanGraphType)).Description("Controls if the participants see the leader board");
            Field(x => x.ShowLeaderBoardStepCounts, type: typeof(BooleanGraphType)).Description("Controls if the participants see the total step counts for other teams on the leader board");
            Field(x => x.NumberOfParticipants, type: typeof(IntGraphType)).Description("How many people are taking part in the challenge");
            Field(x => x.NumberOfParticipantsInATeam, type: typeof(IntGraphType)).Description("How many people should be in a team on average");
            
        }
    }
}
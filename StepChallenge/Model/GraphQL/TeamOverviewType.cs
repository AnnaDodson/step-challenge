using GraphQL.Authorization;
using GraphQL.Types;
using StepChallenge.DataModels;

namespace Model.GraphQL
{
    public class TeamOverviewType : ObjectGraphType<TeamScoresOverview>
    {

        public TeamOverviewType()
        {
            Name = "TeamOverview";
            
            Field(x => x.TeamId, type: typeof(IdGraphType)).Description("The ID of the Team.");
            Field(x => x.TeamName).Description("The name of the Team");
            Field(x => x.NumberOfParticipants, type: typeof(IntGraphType)).Description("Number of Participants in the team");
            Field(x => x.TeamTotalSteps, type: typeof(IntGraphType)).Description(("Total number of team steps"));
            Field(x => x.TeamTotalStepsWithAverage, type: typeof(IntGraphType)).Description(("Total number of team steps"));
            Field(x => x.ParticipantsStepsOverviews, type: typeof(ListGraphType<ParticipantOverviewType>)).Description("Users in the Team with their steps").AuthorizeWith("AdminPolicy");
        }
        
    }
}
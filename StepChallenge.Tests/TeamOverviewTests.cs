using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;
using Moq;
using NUnit.Framework;
using StepChallenge.Services;

namespace StepChallenge.Tests
{
    public class TeamOverviewTests : BaseTests
    {
        /// <summary>
        /// </summary>
        [Test]
        public void Test_TeamMembersStepStatus_IsTrueIfStepsAreSaved()
        {
            var team = TestOverviewData.GetTeams().ToList();

            var stepsService = new StepsService(GetMockStepContext(team));
            var result = stepsService.GetTeamsOverview();

            var expected = true;

            Assert.IsTrue(expected, $"Expected team overview to to have steps of true but got {result}");
        }

        private StepContext GetMockStepContext(List<Team> teams)
        {
            var mockContext = new Mock<StepContext>();
            
            mockContext.Setup(x => x.Team).Returns(GetDbSetTeams(teams).Object);

            return mockContext.Object;
        }
        

        private Mock<DbSet<Team>> GetDbSetTeams(List<Team> teamList)
        {
            var teams = teamList.AsQueryable();

            var mockParticipantSet = new Mock<DbSet<Team>>();
            mockParticipantSet.As<IQueryable<Team>>().Setup(m => m.Expression).Returns(teams.Expression);
            mockParticipantSet.As<IQueryable<Team>>().Setup(m => m.Provider).Returns(teams.Provider);
            mockParticipantSet.As<IQueryable<Team>>().Setup(m => m.Expression).Returns(teams.Expression);
            mockParticipantSet.As<IQueryable<Team>>().Setup(m => m.ElementType).Returns(teams.ElementType);
            mockParticipantSet.As<IQueryable<Team>>().Setup(m => m.GetEnumerator()).Returns(teams.GetEnumerator());

            return mockParticipantSet;
        }
    }
}
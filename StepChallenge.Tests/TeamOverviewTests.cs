using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Test a teams step total is counted correctly
        /// </summary>
        [Test]
        public async Task Test_TeamOverview_TotalSteps_AreCountedCorrectly()
        {
            var team = TestOverviewData.GetTeams().ToList();

            var stepsService = new StepsServiceStub(GetMockStepContext(team));
            var result = await stepsService.GetTeamsOverview();

            var resultTotal = result.ToList().First(r => r.TeamId == 1).TeamTotalSteps;
            var expectedTotal = 180;

            Assert.IsTrue(resultTotal == expectedTotal, $"Expected total steps to be: {expectedTotal} but got {resultTotal}");
        }

        /// <summary>
        /// Test a teams step total is counted correctly and ignore steps counted outside of the challenge dates
        /// </summary>
        [Test]
        [TestCase("01/01/2012")]
        [TestCase("01/01/2020")]
        public async Task Test_TeamOverview_TotalSteps_DoNotCountStepsOutsideOfDates(DateTime date)
        {
            var team = TestOverviewData.GetTeams().ToList();
            team.First(r => r.TeamId == 1).Participants.First(p => p.ParticipantId == 11).Steps.Add(new Steps
            {
                StepCount = 30,
                DateOfSteps = date,
            });

            var stepsService = new StepsServiceStub(GetMockStepContext(team));
            var result = await stepsService.GetTeamsOverview();

            var resultTotal = result.ToList().First(r => r.TeamId == 1).TeamTotalSteps;
            var expectedTotal = 180;

            Assert.IsTrue(resultTotal == expectedTotal, $"Expected total steps to be: {expectedTotal} but got {resultTotal}");
        }

        /// <summary>
        /// Test a teams step total adds an average person correctly for teams with less people
        /// </summary>
        [Test]
        public async Task Test_TeamOverview_TotalSteps_TeamsWithLessPeopleGetAverageSteps()
        {
            var team = TestOverviewData.GetTeamWithThreePeople().ToList();

            var averageTeamSize = 4;
            var stepsService = new StepsServiceStub(GetMockStepContext(team), averageTeamSize);
            var result = await stepsService.GetTeamsOverview();

            var resultTotal = result.ToList().First(r => r.TeamId == 1).TeamTotalStepsWithAverage;
            var expectedTotal = 240;

            Assert.IsTrue(resultTotal == expectedTotal, $"Expected total steps to be: {expectedTotal} but got {resultTotal}");
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
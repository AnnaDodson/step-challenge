using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using StepChallenge.Services;

namespace StepChallenge.Tests
{ 
    
    public class LeaderboardTests : BaseTests
    {
        [Test]
        public void Test_HighestScore()
        {
            var teams = TestData.GetTeams();
            var stepsService = new StepsService(GetMockContext());
            
            var result = stepsService.GetTeamScores(teams, DateOfFirstWeek, StartDate, 3);
            var firstPlace = result.First();

            const int expectedTeamId = 3;
            const int expectedTotal = (3*30)*3;
            
            Assert.AreEqual(expectedTeamId, firstPlace.TeamId, $"Expected TeamId {expectedTeamId} but got {firstPlace.TeamId} instead");
            Assert.AreEqual(expectedTotal, firstPlace.TeamStepCount, $"Expected TeamStepCount {expectedTotal} but got {firstPlace.TeamStepCount} instead");
        }

        [Test]
        public void Test_StepsOutOfDateRange_DoNotGetCounted()
        {
            var teams = TestData.GetTeams_StepsOutsideOfRange();
            var stepsService = new StepsService(GetMockContext());

            var result = stepsService.GetTeamScores(teams, DateOfFirstWeek, StartDate, 3);
            var firstPlace = result.First();
            const int expectedTeamId = 3;
            const int expectedTotal = (3*30)*3;

            Assert.AreEqual(expectedTeamId, firstPlace.TeamId, $"Expected TeamId {expectedTeamId} but got {firstPlace.TeamId} instead");
            Assert.AreEqual(expectedTotal, firstPlace.TeamStepCount, $"Expected TeamStepCount {expectedTeamId} but got {firstPlace.TeamStepCount} instead");
        }

        [Test]
        public void Test_StepsOnlyCountedToLatestMonday()
        {
            var teams = TestData.GetTeams_StepsOverLeadBoardDate();
            var stepsService = new StepsService(GetMockContext());

            var result = stepsService.GetTeamScores(teams, DateOfFirstWeek, StartDate, 3);
            var firstPlace = result.First();
            const int expectedTeamId = 3;
            const int expectedTotal = (3*30)*3;

            Assert.AreEqual(expectedTeamId, firstPlace.TeamId, $"Expected TeamId {expectedTeamId} but got {firstPlace.TeamId} instead");
            Assert.AreEqual(expectedTotal, firstPlace.TeamStepCount, $"Expected TeamStepCount {expectedTeamId} but got {firstPlace.TeamStepCount} instead");
        }

        [Test]
        public void Test_TeamsWithLessParticipants_GetAnExtraAveragedPerson()
        {
            var teams = TestData.GetTeams_OneLessParticipantInTeamOne();
            var stepsService = new StepsService(GetMockContext());

            var result = stepsService.GetTeamScores(teams, DateOfFirstWeek, StartDate, 3);
            var teamWithLessParticipants = result.FirstOrDefault(t => t.TeamId == 1);
            const int teamActualTotal = (3*30)*2;
            const int expectedTotal = (teamActualTotal / 2)* 1;

            Assert.AreEqual(expectedTotal, teamWithLessParticipants?.TeamStepCount, $"Expected TeamStepCount {expectedTotal} but got {teamWithLessParticipants?.TeamStepCount} instead");
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;
using Moq;
using NUnit.Framework;
using StepChallenge.Services;

namespace StepChallenge.Tests
{
    public class ParticipantStepsTests : BaseTests
    {
        [Test]
        public void Test_StepsAreCorrectEachDay()
        {
            var participant = TestData.GetParticipant();

            const int expectedCountPerDay = 10;
            var expectedDays = new List<DateTime>
            {
                StartDate,
                StartDate.AddDays(1),
                StartDate.AddDays(2),
            };
            
            var stepsService = new StepsService(GetMockStepContext(participant.Steps));
            var result = stepsService.GetParticipantSteps(participant);
            
            Assert.That(result.Steps.All(s => s.StepCount == expectedCountPerDay), $"Expected every stepCount to equal {expectedCountPerDay}");
            Assert.That(result.Steps.All(s => expectedDays.Any(e => e == s.DateOfSteps)), $"Expected DateOfSteps to equal {expectedDays}");
        }

        [Test]
        public void Test_StepsOutsideOfDateRangeDoNotCount()
        {
            var participant = TestData.GetParticipant();
            participant.Steps.Add(new Steps{
                StepCount = 10,
                DateOfSteps = StartDate.AddDays(-1)
            });

            const int expectedCountPerDay = 10;
            List<DateTime> expectedDays = new List<DateTime>
            {
                StartDate,
                StartDate.AddDays(1),
                StartDate.AddDays(2),
            };
            
            var stepsService = new StepsService(GetMockStepContext(participant.Steps));
            var result = stepsService.GetParticipantSteps(participant);
            
            Assert.That(result.Steps.All(s => s.StepCount == expectedCountPerDay), $"Expected every stepCount to equal {expectedCountPerDay}");
            Assert.That(result.Steps.All(s => expectedDays.Any(e => e == s.DateOfSteps)), $"Expected DateOfSteps to equal {expectedDays}");
        }

        private StepContext GetMockStepContext(ICollection<Steps> stepsCollection)
        {
            var steps = stepsCollection.AsQueryable();
            var mockContext = new Mock<StepContext>();

            var mockStepsDb = new Mock<DbSet<Steps>>();
            mockStepsDb.As<IQueryable<Steps>>().Setup(m => m.Expression).Returns(steps.Expression);
            mockStepsDb.As<IQueryable<Steps>>().Setup(m => m.Provider).Returns(steps.Provider);
            mockStepsDb.As<IQueryable<Steps>>().Setup(m => m.Expression).Returns(steps.Expression);
            mockStepsDb.As<IQueryable<Steps>>().Setup(m => m.ElementType).Returns(steps.ElementType);
            mockStepsDb.As<IQueryable<Steps>>().Setup(m => m.GetEnumerator()).Returns(steps.GetEnumerator());
            
            mockContext.Setup(x => x.Steps).Returns(mockStepsDb.Object);

            return mockContext.Object;
        }

    }
}
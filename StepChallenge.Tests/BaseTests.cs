using System;
using Moq;

namespace StepChallenge.Tests
{
    public class BaseTests
    {
        protected static readonly DateTime StartDate = new DateTime(2019,09,16, 0,0,0);
        protected static readonly DateTime DateOfFirstWeek = StartDate.AddDays(7);
        
        protected static StepContext GetMockContext()
        {
            return new Mock<StepContext>().Object;
        }
    }
}
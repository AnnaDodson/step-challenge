using System.Threading.Tasks;
using StepChallenge.Services;

namespace StepChallenge.Tests
{
    public class StepsServiceStub : StepsService
    {
        private readonly int _averageTeamSize;
        public StepsServiceStub(StepContext stepContext, int averageTeamSize = 3) : base(stepContext)
        {
            _averageTeamSize = averageTeamSize;
        }

        public override async Task<int> GetAverageTeamSize()
        {
            return _averageTeamSize;
        }
    }
}
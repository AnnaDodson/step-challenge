using System;
using System.Collections.Generic;

namespace StepChallenge.DataModels
{
    public class ParticipantsStepsOverview
    {
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public List<StepsOverview> StepsOverviews { get; set; }

        public int StepTotal { get; set; }
        
    }

    public class StepsOverview
    {
        public int StepCount { get; set; }
        public DateTimeOffset DateOfSteps { get; set; }
    }
}
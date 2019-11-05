using System;
using System.Collections.Generic;
using Model;

namespace StepChallenge.DataModels
{
    public class TeamScoreBoard
    {
        public DateTime DateOfSteps { get; set; }
        public int StepCount { get; set; }
        public List<ParticipantsStepsStatus> ParticipantsStepsStatus { get; set; }
    }

    public class ParticipantsStepsStatus
    {
        public string ParticipantName { get; set; }
        public int ParticipantId { get; set; }
        public bool ParticipantAddedStepCount { get; set; }
        public int ParticipantStepCount { get; set; }
        public bool ParticipantHighestStepper { get; set; }
    }
}
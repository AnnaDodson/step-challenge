using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Steps
    {
        public int StepsId { get; set; }
        public int StepCount { get; set; }
        public DateTimeOffset DateOfSteps { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
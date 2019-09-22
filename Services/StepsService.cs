using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using StepChallenge.DataModels;
using StepChallenge.Mutation;

namespace StepChallenge.Services
{
    public class StepsService
    {
        public StepContext _stepContext;
        public DateTime _startDate = new DateTime(2019,09,16, 0,0,0);
        public DateTime _endDate = new DateTime(2019, 12, 05,0,0,0);

        public StepsService(StepContext stepContext)
        {
            _stepContext = stepContext;
        }
        
        public async Task<Steps> CreateAsync(StepsInputs steps)
        {
            var challengeStartDate = new DateTime(2019,09,16);
            var currentCulture = CultureInfo.CurrentCulture;
            
            var challengeStartWeek = currentCulture.Calendar.GetWeekOfYear(
                new DateTime(challengeStartDate.Year, challengeStartDate.Month, challengeStartDate.Day),
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);
            
            var stepsWeekNo = currentCulture.Calendar.GetWeekOfYear(
                new DateTime(steps.DateOfSteps.Year, steps.DateOfSteps.Month, steps.DateOfSteps.Day),
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);

            var weekNo = (stepsWeekNo - challengeStartWeek) + 1;

            var stepsDay = new DateTime(steps.DateOfSteps.Year, steps.DateOfSteps.Month, steps.DateOfSteps.Day, 0,
                0, 0);
            
            var newSteps = new Steps
            {
                DateOfSteps = stepsDay,
                StepCount = steps.StepCount,
                Day = (int)steps.DateOfSteps.DayOfWeek,
                ParticipantId = steps.ParticipantId,
                Week = weekNo,
            };

            var existingStepCount = await _stepContext.Steps
                .Where(s => s.ParticipantId == steps.ParticipantId)
                .Where(s => s.DateOfSteps == stepsDay)
                .SingleOrDefaultAsync();

            if (existingStepCount != null)
            {
                existingStepCount. StepCount = newSteps.StepCount;
                _stepContext.Steps.Update(existingStepCount);
                await _stepContext.SaveChangesAsync();
                return existingStepCount;
            }

            _stepContext.Steps.Add(newSteps);
            await _stepContext.SaveChangesAsync();
            return newSteps;
        }

        public List<Steps> GetAllWeeksSteps(List<Steps> steps)
        {
            var days = new List<Steps>();
            for (var dt = _startDate; dt <= _endDate; dt = dt.AddDays(1))
            {
                var teamStepDay = steps.Where(s => s.DateOfSteps == dt).ToList();
                var dayStepCount = new Steps
                {
                    StepCount = teamStepDay.Sum(s => s.StepCount),
                    DateOfSteps = dt,
                };
                days.Add(dayStepCount);
            }

            return days;
        }

    }
}
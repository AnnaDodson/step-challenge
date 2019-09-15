using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using StepChallenge.Mutation;

namespace StepChallenge.Services
{
    public class StepsService
    {
        public StepContext _stepContext;

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
            
            var newSteps = new Steps
            {
                DateOfSteps = steps.DateOfSteps,
                StepCount = steps.StepCount,
                Day = (int)steps.DateOfSteps.DayOfWeek,
                UserId = steps.UserId,
                Week = weekNo
            };

            var existingStepCount = await _stepContext.Steps
                .Where(s => s.UserId == steps.UserId)
                .Where(s => s.DateOfSteps.Day == steps.DateOfSteps.Day)
                .SingleOrDefaultAsync();

            if (existingStepCount != null)
            {
                existingStepCount.StepCount = newSteps.StepCount;
                await _stepContext.SaveChangesAsync();
                return existingStepCount;
            }

            _stepContext.Steps.Add(newSteps);
            await _stepContext.SaveChangesAsync();
            return newSteps;
        }
    }
}
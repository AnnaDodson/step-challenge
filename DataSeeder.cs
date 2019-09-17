using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StepChallenge
{
    public class DataSeed
    {
        public static void Run(StepContext db)
        {
            if (!db.Team.Any())
            {
                var teams = new List<Team>
                {
                    new Team
                    {
                        TeamId = 1,
                        Name = "TeamOne",
                        Users = new[]
                        {
                            new User
                            {
                                UserId = 1,
                                UserName = "Alice",
                                IsAdmin = true,
                                Steps = GetSteps()
                            },
                            new User
                            {
                                UserId = 2,
                                UserName = "Bob",
                                Steps = GetSteps()
                            }
                        }
                    },
                    new Team
                    {
                        TeamId = 2,
                        Name = "TeamTwo",
                        Users = new[]
                        {
                            new User
                            {
                                UserId = 3,
                                UserName = "Susan",
                                Steps = GetSteps()
                            },
                            new User
                            {
                                UserId = 4,
                                UserName = "Helga",
                                Steps = GetSteps()
                            }
                        }
                    }
                };
                    
                db.Team.AddRange(teams);
                db.SaveChanges();
            }
        }

        private static List<Steps> GetSteps()
        {
            var week = 1;
            var monday = new DateTime(2019, 9, 16, 0, 0, 0);
            var steps = new List<Steps>
            {
                new Steps{
                    DateOfSteps = monday,
                    StepCount = GenerateRandomSteps(),
                    Week = week,
                    Day = 1,
                },
                new Steps{
                    DateOfSteps = monday.AddDays(1),
                    StepCount = GenerateRandomSteps(),
                    Week = week,
                    Day = 2,
                },
                new Steps{
                    DateOfSteps = monday.AddDays(5),
                    StepCount = GenerateRandomSteps(),
                    Week = week,
                    Day = 6,
                },
                new Steps{
                    DateOfSteps = monday.AddDays(4),
                    StepCount = GenerateRandomSteps(),
                    Week = week,
                    Day = 5,
                },
            };
            return steps;

            int GenerateRandomSteps()
            {
                Random rnd = new Random();
                return rnd.Next(0, 10); 
            }
            
        }

    }
}
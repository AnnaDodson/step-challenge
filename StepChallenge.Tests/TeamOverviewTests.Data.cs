using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using NUnit.Framework;

namespace StepChallenge.Tests
{
    public class TestOverviewData
    {
        private static DateTime StartDate = new DateTime(2019,09,16, 0,0,0);
        
        [SetUp]
        public void Setup()
        {
        }

        public static IQueryable<Team> GetTeams(){
            var teams = CreateThreeTeams();
            return teams;
        }

        public static IQueryable<Team> GetTeamWithThreePeople()
        {
            var teams = CreateTeamWithOneLessPerson();
            return teams;
        }

        private static IQueryable<Team> CreateTeamWithOneLessPerson()
        {
            return (new List<Team>
            {
                new Team
                {
                    TeamId = 1,
                    TeamName = "Team_1",
                    NumberOfParticipants = 3,
                    Participants = CreateParticipants(10)
                }
            }).AsQueryable();
        }

        private static IQueryable<Team> CreateThreeTeams()
        {
            return (new List<Team>
            {
                new Team
                {
                    TeamId = 1,
                    TeamName = "Team_1",
                    NumberOfParticipants = 3,
                    Participants = CreateParticipants(10)
                },
                new Team
                {
                    TeamId = 2,
                    TeamName = "Team_2",
                    NumberOfParticipants = 3,
                    Participants = CreateParticipants(20)
                },
                new Team
                {
                    TeamId = 3,
                    TeamName = "Team_3",
                    NumberOfParticipants = 3,
                    Participants = CreateParticipants(30)
                }
            }).AsQueryable();
        }

        private static ICollection<Participant> CreateParticipants(int id)
        {
            var participants = new List<Participant>
            {
                new Participant
                {
                    ParticipantName = "ParticipantNameOne",
                    ParticipantId = id + 1,
                    Steps = CreateSteps(20)
                },
                new Participant
                {
                    ParticipantName = "ParticipantNameTwo",
                    ParticipantId = id + 2,
                    Steps = CreateSteps(20)
                },
                new Participant
                {
                    ParticipantName = "ParticipantNameThree",
                    ParticipantId = id + 3,
                    Steps = CreateSteps(20)
                },
            };
            return participants;
        }

        private static ICollection<Steps> CreateSteps(int stepCount)
        {
            var steps = new List<Steps>();
            
            for (int i = 0; i < 3; i++)
            {
                steps.Add(
                    new Steps
                    {
                        StepCount = stepCount,
                        DateOfSteps = StartDate.AddDays(i),
                    }
                );
            }

            return steps;
        }
    }
}
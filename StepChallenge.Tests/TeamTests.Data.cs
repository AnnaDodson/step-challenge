using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NUnit.Framework;

namespace StepChallenge.Tests
{
    public class LeaderBoardTestsData
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

        public static IQueryable<Team> GetTeams_StepsOutsideOfRange(){
            var teams = CreateThreeTeams();
            
            var stepOutOfDateRange = new Steps
            {
                StepCount = 1000,
                DateOfSteps = StartDate.AddDays(-4)
            };
            
            teams.First().Participants.First().Steps.Add(stepOutOfDateRange);
            return teams;
        }
        
        public static IQueryable<Team> GetTeams_OneLessParticipantInTeamOne()
        {
            var teams = CreateThreeTeams();

            var first = teams.FirstOrDefault(t => t.TeamId == 1)?.Participants.First();
            teams.First().Participants.Remove(first);
            teams.First().NumberOfParticipants = 2;

            return teams;
        }

        public static IQueryable<Team> GetTeams_StepsOverLeadBoardDate()
        {
            var teams = CreateThreeTeams();
            
            var first = teams.FirstOrDefault(t => t.TeamId == 1)?.Participants.First();
            first?.Steps.Add
            (
                new Steps
                {
                    StepCount = 10000,
                    DateOfSteps = StartDate.AddDays(8)
                }
            );

            return teams;
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
                    Participants = GetParticipants_TeamOne()
                },
                new Team
                {
                    TeamId = 2,
                    TeamName = "Team_2",
                    NumberOfParticipants = 3,
                    Participants = GetParticipants_TeamTwo()
                },
                new Team
                {
                    TeamId = 3,
                    TeamName = "Team_3",
                    NumberOfParticipants = 3,
                    Participants = GetParticipants_TeamThree()
                }
            }).AsQueryable();

        }

        private static ICollection<Participant> GetParticipants_TeamOne()
        {
            var participants = CreateParticipants();
            foreach (var participant in participants)
            {
                participant.Steps = CreateSteps(10);
            }

            return participants;
        }

        private static ICollection<Participant> GetParticipants_TeamTwo()
        {
            var participants = CreateParticipants();
            foreach (var participant in participants)
            {
                participant.Steps = CreateSteps(20);
            }
            return participants;
        }

        private static ICollection<Participant> GetParticipants_TeamThree()
        {
            var participants = CreateParticipants();
            foreach (var participant in participants)
            {
                participant.Steps = CreateSteps(30);
            }
            return participants;
        }

        private static ICollection<Participant> CreateParticipants()
        {
            var participants = new List<Participant>
            {
                new Participant
                {
                    ParticipantName = "ParticipantNameOne",
                },
                new Participant
                {
                    ParticipantName = "ParticipantNameTwo",
                },
                new Participant
                {
                    ParticipantName = "ParticipantNameThree",
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
                        DateOfSteps = StartDate.AddDays(i)
                    }
                );
            }

            return steps;
        }
        
    }
}
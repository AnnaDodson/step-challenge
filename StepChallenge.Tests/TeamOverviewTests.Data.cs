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

        public static Participant GetParticipant()
        {
            var participant = new Participant
            {
                ParticipantName = "ParticipantNameOne",
                ParticipantId = 1,
                Steps = CreateSteps(10, 1),
                Team = new Team
                {
                    TeamName = "Team Name"
                }
            };
            return participant;
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
                    ParticipantId = 1,
                },
                new Participant
                {
                    ParticipantName = "ParticipantNameTwo",
                    ParticipantId = 2,
                },
                new Participant
                {
                    ParticipantName = "ParticipantNameThree",
                    ParticipantId = 3,
                },
            };
            return participants;
        }

        private static ICollection<Steps> CreateSteps(int stepCount, int participantId = 0)
        {
            var steps = new List<Steps>();
            
            for (int i = 0; i < 3; i++)
            {
                steps.Add(
                    new Steps
                    {
                        StepCount = stepCount,
                        DateOfSteps = StartDate.AddDays(i),
                        ParticipantId = participantId
                    }
                );
            }

            return steps;
        }
    }
}
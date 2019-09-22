using System.Collections.Generic;

namespace Model
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public ICollection<Participant> Participants { get; set; }
    }
}
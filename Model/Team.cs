using System.Collections.Generic;

namespace Model
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
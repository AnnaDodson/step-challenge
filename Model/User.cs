using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
        
        public ICollection<Steps> Steps { get; set; }
    }
}
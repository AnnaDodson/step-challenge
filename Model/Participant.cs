using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using Microsoft.AspNetCore.Identity;

namespace Model
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public bool IsAdmin { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
        
        [ForeignKey("Id")]
        public virtual IdentityUser IdentityUser { get; set; }
        public ICollection<Steps> Steps { get; set; }
    }
}
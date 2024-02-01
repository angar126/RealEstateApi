using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstateService.Models
{
    public partial class Issue
    {
        public Issue()
        {
            Comments = new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public string AbitazioneId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public string CreatedByUserId { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}

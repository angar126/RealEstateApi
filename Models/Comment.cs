using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstateService.Models
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public string CommentDescription { get; set; }
        public Guid IssueId { get; set; }
        public string UserId { get; set; }

        public virtual Issue Issue { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstateService.Models
{
    public partial class House
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Mq { get; set; }
        public string Description { get; set; }
        public string? UserTenantId { get; set; }
    }
}

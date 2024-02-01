using System;

namespace RealEstateService.Models.ModelsDTO
{
    public class HouseDTO
    {
        public HouseDTO(House house)
        {
            Id = house.Id;
            UserId = house.UserId;
            Address = house.Address;
            City = house.City;
            Mq = house.Mq;
            Description = house.Description;
            UserTenantId = house.UserTenantId;
        }
        public HouseDTO()
        { }

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Mq { get; set; }
        public string Description { get; set; }
        public string? UserTenantId { get; set; }
    }
}

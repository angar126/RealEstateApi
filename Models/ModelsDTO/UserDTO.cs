using System;

namespace RealEstateService.Models.ModelsDTO
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            LastName = user.LastName;
            Role = user.Role;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}

using System;

namespace RealEstateApi.Models.ResponseModelsDTO
{
    public class ResponseUserDTO
    {
        public ResponseUserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            LastName = user.LastName;
            Email = user.Email;
           
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }


    }
}

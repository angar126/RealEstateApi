using RealEstateApi.Models.ModelsDTO;

namespace RealEstateApi.Models.RequestModelsDTO
{
    public class RequestRegisterDTO
    {
        public RequestUserDTO DataAccount { get; set; }
        public LoginDTO Credentials { get; set; }
    }
    
}

using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.RequestModelsDTO;
using System.Threading.Tasks;

namespace RealEstateApi.Repositories.Interfaces
{
    public interface IAutentication

    {
        public Task<string> Login(LoginDTO credentials);
        public Task<UserDTO> Logout(LoginDTO credentials);

        public Task<UserDTO> Register(RequestRegisterDTO request);
        public string GenerateJwtToken(UserDTO user);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);


    }
}

using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.ResponseModelsDTO;
using System;
using System.Threading.Tasks;

namespace RealEstateApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<ResponseUserDTO> GetByID(string id);

        public Task<bool> UserExist(string id);
    }
}

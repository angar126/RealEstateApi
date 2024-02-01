using Microsoft.EntityFrameworkCore;
using RealEstateApi.Models;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.RequestModelsDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApi.Repositories.Interfaces
{
    public interface IHouseRepository
    {
        public Task<IEnumerable<HouseDTO>> GetAllAsync();
        public Task<IEnumerable<HouseDTO>> GetAllHouseByIdOwnerAsync(Guid id);
        public Task<IEnumerable<HouseDTO>> GetAllHouseByCityAsync(string city);
        public Task<HouseDTO> GetByIdAsync(Guid id);
        public Task<House> AddAsync(RequestHouseDTO entity);
        public Task<HouseDTO> UpdateAsync(HouseDTO entity);
        public Task<HouseDTO> DeleteAsync(Guid id);
    }
}

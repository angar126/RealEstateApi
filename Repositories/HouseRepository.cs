using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Models.RequestModelsDTO;

namespace RealEstateApi.Repository
{
    public class HouseRepository  : IHouseRepository
    {
        protected readonly AirbnbDataContext _context;
        private readonly ILogger<HouseRepository> _logger;
        public HouseRepository(AirbnbDataContext context, ILogger<HouseRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<HouseDTO>> GetAllAsync()
        {
            try
            {
                var houses = await _context.Houses.ToListAsync();
                if (!houses.Any())
                {
                    _logger.LogInformation("No houses found");
                    return null;
                }
                return houses.Select(i => new HouseDTO(i));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving data from House tables");
                throw;
            }
        }

        public async Task<HouseDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var house = await _context.Houses.FirstOrDefaultAsync(a => a.Id.Equals(id));

                if (house == null)
                {
                    _logger.LogInformation($"No House with id: {id}");
                    return null;
                }

                return new HouseDTO(house);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error while trying to retrieve data from House tables");
                throw;
            }
        }
        public async Task<IEnumerable<HouseDTO>> GetAllHouseByIdOwnerAsync(Guid userId)
        {
            try
            {
                var houses = await _context.Houses.Where(i => i.UserId.Equals(userId)).ToListAsync();
                if (!houses.Any())
                {
                    _logger.LogInformation("No houses found");
                    return null;
                }
                return houses.Select(i => new HouseDTO(i));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving data from House tables");
                throw;
            }
        }
        public async Task<IEnumerable<HouseDTO>> GetAllHouseByCityAsync(string city)
        {
            try
            {
                var houses = await _context.Houses
                                           .Where(i => i.City.Equals(city) && i.UserTenantId == null)
                                           .ToListAsync();

                if (!houses.Any())
                {
                    _logger.LogInformation("No available houses found in the specified city.");
                    return null;
                }

                return houses.Select(i => new HouseDTO(i));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data from House tables");
                throw;
            }
        }

        public async Task<House> AddAsync(RequestHouseDTO houseDTO)
        {
            try
            {
                House house = new House
                {
                    Id = Guid.NewGuid(),
                    UserId = houseDTO.UserId,
                    Address = houseDTO.Address,
                    City = houseDTO.City,
                    Mq = houseDTO.Mq,
                    Description = houseDTO.Description,
                    UserTenantId = houseDTO.UserTenantId,
                };
                await _context.Houses.AddAsync(house);
                await _context.SaveChangesAsync();
                return house;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding a house: {ex.Message}");
                throw;
            }
        }

        public async Task<HouseDTO> UpdateAsync(HouseDTO house)
        {
            try
            {
                var existingHouse = await _context.Houses.FirstOrDefaultAsync(a => a.Id.Equals(house.Id));
                if (existingHouse == null)
                {
                    _logger.LogInformation($"No House with id: {house.Id}");
                    return null;
                }
                _context.Entry(existingHouse).CurrentValues.SetValues(house);
                await _context.SaveChangesAsync();
                return house;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating house with id {house.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<HouseDTO> DeleteAsync(Guid id)
        {
            try
            {
                var house = await _context.Houses.FirstOrDefaultAsync(h => h.Id == id);
                if (house == null)
                {
                    _logger.LogInformation($"No House with id: {id}");
                    return null;
                }

                _context.Houses.Remove(house);
                await _context.SaveChangesAsync();
                return new HouseDTO(house);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting house with id {id}: {ex.Message}");
                throw;

            }
        }

    }
}





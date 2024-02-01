using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Repository;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.Models.RequestModelsDTO;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateApi.Controllers
{
    [ApiController]
    [Route("/api/RealEstate/[controller]")]
    [Authorize]
    public class HouseController : Controller
    {
        private readonly ILogger<HouseRepository> _logger;
        private readonly IHouseRepository _houseRepo;
        public HouseController(ILogger<HouseRepository> logger, IHouseRepository houseRepo)
        {
            _logger = logger;
            _houseRepo = houseRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HouseDTO>> GetById(Guid id)
        {
            try
            {
                var houseResult = await _houseRepo.GetByIdAsync(id);

                if (houseResult == null)
                    return NotFound();

                return Ok(houseResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving a house by ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HouseDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Request for Fetching all houses");
                var result = await _houseRepo.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving all houses: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("/City/{city}", Name = "GetAllHouseByCity")]
        public async Task<ActionResult<IEnumerable<HouseDTO>>> GetAllHouseByCity(string city)
        {
            try
            {
                _logger.LogInformation($"Request for Fetching all houses by city");
                var result = await _houseRepo.GetAllHouseByCityAsync(city);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving houses by city: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("/Owner/{id}")]
        public async Task<ActionResult<IEnumerable<HouseDTO>>> GetAllHouseByIdOwnerAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Request for Fetching all houses by OwnerId");
                var result = await _houseRepo.GetAllHouseByIdOwnerAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving houses by OwnerId: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestHouseDTO house)
        {
            try
            {
                _logger.LogInformation($"Request for Creating a house");
                await _houseRepo.AddAsync(house);
                return CreatedAtAction(nameof(GetById), new { id = Guid.NewGuid() }, house);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a house: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, string description)
        {
            try
            {
                var existingHouse = await _houseRepo.GetByIdAsync(id);
                if (existingHouse is null)
                    return NotFound();

                _logger.LogInformation($"Request for Updating a house");
                var updatedHouse = new HouseDTO
                {
                    Id = existingHouse.Id,
                    UserId = existingHouse.UserId,
                    Address = existingHouse.Address,
                    City = existingHouse.City,
                    Mq = existingHouse.Mq,
                    Description = description,
                    UserTenantId = existingHouse.UserTenantId,
                };
                await _houseRepo.UpdateAsync(updatedHouse);
                var response = new
                {
                    message = $"House updated correctly",
                    body= updatedHouse,
                    success = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating a house: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPut("/houses/tenant/{id}")]
        public async Task<IActionResult> UpdateTenant(Guid id, string tenantId)
        {
            try
            {

                if (!Guid.TryParse(tenantId, out Guid issueId))
                {
                    return BadRequest("Invalid Issue ID.");
                }

                var existingHouse = await _houseRepo.GetByIdAsync(id);
                if (existingHouse is null)
                    return NotFound();

                _logger.LogInformation($"Request for Updating tenant");
                var updatedHouse = new HouseDTO
                {
                    Id = existingHouse.Id,
                    UserId = existingHouse.UserId,
                    Address = existingHouse.Address,
                    City = existingHouse.City,
                    Mq = existingHouse.Mq,
                    Description = existingHouse.Description,
                    UserTenantId = tenantId,
                };
                await _houseRepo.UpdateAsync(updatedHouse);
                var response = new
                {
                    message = $"tenant updated correctly",
                    body = updatedHouse,
                    success = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the tenant: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }



        [HttpPut("/houses/tenantdelete/{id}")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            try
            {

                var existingHouse = await _houseRepo.GetByIdAsync(id);
                if (existingHouse is null)
                    return NotFound();

                _logger.LogInformation($"Request for Updating tenant");
                var updatedHouse = new HouseDTO
                {
                    Id = existingHouse.Id,
                    UserId = existingHouse.UserId,
                    Address = existingHouse.Address,
                    City = existingHouse.City,
                    Mq = existingHouse.Mq,
                    Description = existingHouse.Description,
                    UserTenantId = null,
                };
                await _houseRepo.UpdateAsync(updatedHouse);
                var response = new
                {
                    message = $"tenant deleted correctly",
                    body = updatedHouse,
                    success = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the tenant: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var house = await _houseRepo.GetByIdAsync(id);

                if (house is null)
                    return NotFound();

                _logger.LogInformation($"Request for Deleting a house");
                await _houseRepo.DeleteAsync(house.Id);
                var response = new
                {
                    message = $"House deleted correctly",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting a house: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

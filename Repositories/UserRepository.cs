using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models.Interfaces;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.ResponseModelsDTO;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace RealEstateApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly UserAirbnbDataContext _context;
        public readonly ILogger<UserRepository> _logger;

        public UserRepository(UserAirbnbDataContext context, ILogger<UserRepository> Logger)

        {
            _context = context;
            _logger = Logger;
        }

      
        public async Task<ResponseUserDTO> GetByID(string id)
        {
            try
            {   

                Guid idParse = new Guid();

                if (!Guid.TryParse(id,out idParse))
                {
                    _logger.LogInformation("Invalid id");

                    return null;
                }
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == idParse);
                
                if (user == null)
                {
                    _logger.LogInformation($"No user whit ID {id}");
                    return null;
                }
                return new ResponseUserDTO(user);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving user {id}");

                throw;
            }
        }

        public async Task<bool> UserExist(string id)
        {
            try
            {
                Guid idParse = new Guid();

                if (!Guid.TryParse(id, out idParse))
                {
                    _logger.LogInformation("Invalid id");

                    return false;
                }


                if (await _context.Users.AnyAsync(u => u.Id == idParse))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving user {id}");

                throw;
            }

        }

        
    }
}

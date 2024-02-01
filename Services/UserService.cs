using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RealEstateService.Models.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace RealEstateService.Services
{
    public class UserService
    {
        private readonly string _realEstateApiBaseUrl;
        private readonly HttpClient httpClient = RealEstateServiceCollectionExtension.client;
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger, string url)
        {
            _realEstateApiBaseUrl = url;
            _logger = logger;
        }
        public async Task<List<UserDTO>> GetAllUser()
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/User");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<UserDTO>>(responseBody);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Users not found");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Users");
                return null;
            }
        }
        public async Task<UserDTO> GetUserById(Guid Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/User/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserDTO>(responseBody);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"User with Id {Id} not found");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting User with Id {Id}");
                return null;
            }
        }
        public async Task<UserDTO> AddUser(UserDTO userDTO)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(userDTO);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_realEstateApiBaseUrl}/User/", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var addedUserDTO = JsonConvert.DeserializeObject<UserDTO>(responseBody);

                    return addedUserDTO;
                }
                else
                {
                    _logger.LogError($"Error adding User. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding User");
                return null;
            }
        }

        public async Task<UserDTO> UpdateUser(Guid id, UserDTO updatedUser)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(updatedUser);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"{_realEstateApiBaseUrl}/User/{id}", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var updatedUserDTO = JsonConvert.DeserializeObject<UserDTO>(responseBody);

                    return updatedUserDTO;
                }
                else
                {
                    _logger.LogError($"Error updating User with Id {id}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating User with Id {id}");
                return null;
            }
        }
        public async Task<UserDTO> DeleteUser(Guid id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{_realEstateApiBaseUrl}/User/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deletedUserDTO = JsonConvert.DeserializeObject<UserDTO>(responseBody);

                    return deletedUserDTO;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"User with Id {id} not found");
                    return null;
                }
                else
                {
                    _logger.LogError($"Error deleting User with Id {id}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting User with Id {id}");
                return null;
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}

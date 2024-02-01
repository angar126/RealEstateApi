using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RealEstateService.Models.ModelsDTO;
using Microsoft.Extensions.Logging;


namespace RealEstateService.Services
{
    public class HouseService
    {
        private readonly string _realEstateApiBaseUrl;
        private readonly HttpClient httpClient = RealEstateServiceCollectionExtension.client;
        private readonly ILogger<HouseService> _logger;
        public HouseService(ILogger<HouseService> logger, string url)
        {
            _realEstateApiBaseUrl = url;
            _logger = logger;
        }
        public async Task<List<HouseDTO>> GetAllHouse()
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/House");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<HouseDTO>>(responseBody);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Houses not found");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Houses");
                return null;
            }
        }
        public async Task<List<HouseDTO>> GetAllHouseByIdOwner(Guid id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/House/Owner/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<HouseDTO>>(responseBody);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Houses not found");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Houses");
                return null;
            }
        }
        public async Task<List<HouseDTO>> GetAllHouseByCity(string city)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/House/{city}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<HouseDTO>>(responseBody);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Houses not found");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Houses");
                return null;
            }
        }
        public async Task<HouseDTO> GetHouseById(Guid Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/House/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<HouseDTO>(responseBody);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"House with Id {Id} not found");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting House with Id {Id}");
                return null;
            }
        }
        public async Task<HouseDTO> AddHouse(HouseDTO houseDTO)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(houseDTO);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_realEstateApiBaseUrl}/House/", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var addedHouseDTO = JsonConvert.DeserializeObject<HouseDTO>(responseBody);

                    return addedHouseDTO;
                }
                else
                {
                    _logger.LogError($"Error adding House. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding House");
                return null;
            }
        }

        public async Task<HouseDTO> UpdateHouse(Guid id, HouseDTO updatedHouse)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(updatedHouse);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"{_realEstateApiBaseUrl}/House/{id}", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var updatedHouseDTO = JsonConvert.DeserializeObject<HouseDTO>(responseBody);

                    return updatedHouseDTO;
                }
                else
                {
                    _logger.LogError($"Error updating House with Id {id}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating House with Id {id}");
                return null;
            }
        }
        public async Task<HouseDTO> DeleteHouse(Guid id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{_realEstateApiBaseUrl}/House/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deletedHouseDTO = JsonConvert.DeserializeObject<HouseDTO>(responseBody);

                    return deletedHouseDTO;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"House with Id {id} not found");
                    return null;
                }
                else
                {
                    _logger.LogError($"Error deleting House with Id {id}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting House with Id {id}");
                return null;
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}

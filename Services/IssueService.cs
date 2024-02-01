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
    public class IssueService
    {
        private readonly string _realEstateApiBaseUrl;
        private readonly HttpClient httpClient = RealEstateServiceCollectionExtension.client;
        private readonly ILogger<IssueService> _logger;
        public IssueService(ILogger<IssueService> logger, string url)
        {
            _realEstateApiBaseUrl = url;
            _logger = logger;
        }
        //public async Task<List<IssueDTO>> GetAllIssue()
        //{
        //    try
        //    {
        //        var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/Issue");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<List<IssueDTO>>(responseBody);
        //        }
        //        else
        //        {
        //            if (response.StatusCode == HttpStatusCode.NotFound)
        //            {
        //                _logger.LogWarning("Issues not found");
        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting all Issues");
        //        return null;
        //    }
        //}
        public async Task<IssueDTO> GetIssueById(Guid Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/Issue/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IssueDTO>(responseBody);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Issue with Id {Id} not found");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting Issue with Id {Id}");
                return null;
            }
        }
        public async Task<IssueDTO> GetIssueByHouse(Guid Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IssueDTO>(responseBody);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Issue for House with Id {Id} not found");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting Issue for House  with Id {Id}");
                return null;
            }
        }
        public async Task<IssueDTO> AddIssue(IssueDTO issueDTO)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(issueDTO);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_realEstateApiBaseUrl}/Issue/", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var addedIssueDTO = JsonConvert.DeserializeObject<IssueDTO>(responseBody);

                    return addedIssueDTO;
                }
                else
                {
                    _logger.LogError($"Error adding Issue. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Issue");
                return null;
            }
        }

        //public async Task<IssueDTO> UpdateHouse(Guid id, IssueDTO updatedIssue)
        //{
        //    try
        //    {
        //        var jsonContent = JsonConvert.SerializeObject(updatedIssue);
        //        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //        var response = await httpClient.PutAsync($"{_realEstateApiBaseUrl}/Issue/{id}", stringContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            var updatedIssueDTO = JsonConvert.DeserializeObject<IssueDTO>(responseBody);

        //            return updatedIssueDTO;
        //        }
        //        else
        //        {
        //            _logger.LogError($"Error updating Issue with Id {id}. Status code: {response.StatusCode}");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error updating Issue with Id {id}");
        //        return null;
        //    }
        //}
        //public async Task<IssueDTO> DeleteHouse(Guid id)
        //{
        //    try
        //    {
        //        var response = await httpClient.DeleteAsync($"{_realEstateApiBaseUrl}/Issue/{id}");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            var deletedIssueDTO = JsonConvert.DeserializeObject<IssueDTO>(responseBody);

        //            return deletedIssueDTO;
        //        }
        //        else if (response.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            _logger.LogWarning($"Issue with Id {id} not found");
        //            return null;
        //        }
        //        else
        //        {
        //            _logger.LogError($"Error deleting Issue with Id {id}. Status code: {response.StatusCode}");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error deleting Issue with Id {id}");
        //        return null;
        //    }
        //}

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}

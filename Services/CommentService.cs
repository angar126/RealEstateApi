using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RealEstateService.Models.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace RealEstateService.Services
{
    public class CommentService
    {
        private readonly string _realEstateApiBaseUrl;
        private readonly HttpClient httpClient = RealEstateServiceCollectionExtension.client;
        private readonly ILogger<CommentService> _logger;
        public CommentService(ILogger<CommentService> logger, string url)
        {
            _realEstateApiBaseUrl = url;
            _logger = logger;
        }
        //public async Task<List<CommentDTO>> GetAllComment()
        //{
        //    try
        //    {
        //        var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/Comment");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<List<CommentDTO>>(responseBody);
        //        }
        //        else
        //        {
        //            if (response.StatusCode == HttpStatusCode.NotFound)
        //            {
        //                _logger.LogWarning("Comments not found");
        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting all comments");
        //        return null;
        //    }
        //}
        public async Task<CommentDTO> GetCommentById(Guid Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/Comment/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CommentDTO>(responseBody);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Comment with Id {Id} not found");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting comment with Id {Id}");
                return null;
            }
        }
        public async Task<CommentDTO> GetCommentByIssueId(Guid Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{_realEstateApiBaseUrl}/Comment/Issue/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CommentDTO>(responseBody);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Comment for Issue with Id {Id} not found");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting comment for Issue with Id {Id}");
                return null;
            }
        }
        public async Task<CommentDTO> AddComment(CommentDTO commentDTO)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(commentDTO);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_realEstateApiBaseUrl}/Comment/", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var addedCommentDTO = JsonConvert.DeserializeObject<CommentDTO>(responseBody);

                    return addedCommentDTO;
                }
                else
                {
                    _logger.LogError($"Error adding comment. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return null;
            }
        }

        public async Task<CommentDTO> UpdateComment(Guid id, CommentDTO updatedComment)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(updatedComment);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"{_realEstateApiBaseUrl}/Comment/{id}", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var updatedCommentDTO = JsonConvert.DeserializeObject<CommentDTO>(responseBody);

                    return updatedCommentDTO;
                }
                else
                {
                    _logger.LogError($"Error updating comment with Id {id}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating comment with Id {id}");
                return null;
            }
        }
        public async Task<CommentDTO> DeleteComment(Guid id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{_realEstateApiBaseUrl}/Comment/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deletedCommentDTO = JsonConvert.DeserializeObject<CommentDTO>(responseBody);

                    return deletedCommentDTO;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Comment with Id {id} not found");
                    return null;
                }
                else
                {
                    _logger.LogError($"Error deleting comment with Id {id}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting comment with Id {id}");
                return null;
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}

using RealEstateApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.RequestModelsDTO;
using RealEstateApi.Models.ResponseModelsDTO;
using RealEstateApi.Enumeratori;

namespace RealEstateApi.Repositories.Interfaces
{
    public interface IIssueRepository
    {
        Task<ResponseIssueDTO> GetIssueById(Guid issueId);
        Task<IEnumerable<ResponseIssueDTO>> GetIssuesForHouse(string houseId);
        Task<IssueDTO> CreateIssue(RequestIssueDTO newIssue);
        Task<bool> UpdateIssueStatus(Guid id,IssueStatus newStatus);
        Task<bool> IssueExists(Guid issueId);
    }
}

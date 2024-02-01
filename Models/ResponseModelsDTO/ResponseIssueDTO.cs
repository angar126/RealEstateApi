using System.Collections.Generic;
using System;
using System.Linq;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Enumeratori;

namespace RealEstateApi.Models.ResponseModelsDTO
{
    public class ResponseIssueDTO
    {
        public ResponseIssueDTO()
        {
            
        }
        public ResponseIssueDTO(Issue issue)
        {
            Id = issue.Id;
            HouseId = issue.HouseId;
            UserHouseId = issue.UserHouseId;
            Description = issue.Description;
            State = issue.State;
            Date = issue.Date;
            CreatedByUserId = issue.CreatedByUserId;
            Comments = issue.Comments.Select(c => new CommentDTO(c)).ToList();
        }

        public Guid Id { get; set; }
        public string HouseId { get; set; }
        public string UserHouseId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public string CreatedByUserId { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public IssueStatus Status { get; internal set; }
    }
}

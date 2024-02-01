using RealEstateApi.Models.ModelsDTO;
using System;

namespace RealEstateApi.Models.RequestModelsDTO
{
    public class RequestCommentDTO
    {
        public RequestCommentDTO(CommentDTO DTO)
        {
            CommentDescription = DTO.CommentDescription;
            IssueId = DTO.IssueId;
            UserId = DTO.UserId;
        }
        public RequestCommentDTO() { }
        public string CommentDescription { get; set; }
        public string IssueId { get; set; }
        public string UserId { get; set; }
    }
}

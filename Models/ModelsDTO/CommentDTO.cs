using System;

namespace RealEstateService.Models.ModelsDTO
{
    public class CommentDTO
    {
        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            CommentDescription = comment.CommentDescription;
            IssueId = comment.IssueId.ToString();
            UserId = comment.UserId;
        }


        public CommentDTO() { }
        public Guid Id { get; set; }
        public string CommentDescription { get; set; }
        public string IssueId { get; set; }
        public string UserId { get; set; }
    }
}

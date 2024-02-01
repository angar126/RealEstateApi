using System.Collections.Generic;
using System;

namespace RealEstateService.Models.ModelsDTO
{
    public class IssueDTO
    {
        public IssueDTO(Issue issue)
        {
            Id = issue.Id;
            AbitazioneId = issue.AbitazioneId;
            Description = issue.Description;
            State = issue.State;
            Date = issue.Date;
            CreatedByUserId = issue.CreatedByUserId;
        }
        public IssueDTO()
        {
            
        }
        public Guid Id { get; set; }
        public string AbitazioneId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public string CreatedByUserId { get; set; }
    }
}

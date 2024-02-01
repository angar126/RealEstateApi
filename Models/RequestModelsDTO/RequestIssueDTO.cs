using RealEstateApi.Enumeratori;
using System;

namespace RealEstateApi.Models.RequestModelsDTO
{
    public class RequestIssueDTO
    {
        public string HouseId { get; set; }
        public string UserHouseId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string CreatedByUserId { get; set; }

        public IssueStatus Status { get; set; }
    }
}

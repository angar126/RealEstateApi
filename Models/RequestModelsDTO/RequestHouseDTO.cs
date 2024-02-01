namespace RealEstateApi.Models.RequestModelsDTO
{
    public class RequestHouseDTO
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Mq { get; set; }
        public string Description { get; set; }
        public string? UserTenantId { get; set; }
    }
}

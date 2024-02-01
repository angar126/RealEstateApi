namespace RealEstateApi.Models.Interfaces
{
    public interface INotifier
    {
        public void SendNotification(string toAddress, string subject, string body);
    }
}

using RealEstateApi.Models.ModelsDTO;

namespace RealEstateApi.Models.Interfaces
{
    public interface IMailTemplate
    {
        public string GetMailTemplateForNewComment(string commenterUserName, string receiverUserName, string IssueId);
        public string GetMailTemplateForNewUser(string userName);
        public string GetMailTemplateForNewIssue(string ownerUserName, string issuerUserName);
    }
}

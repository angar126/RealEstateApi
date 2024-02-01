using RealEstateApi.Models.Interfaces;
using RealEstateApi.Models.ModelsDTO;

namespace RealEstateApi.Models.Templates
{
    public class MailTemplates : IMailTemplate
    {

        public string GetMailTemplateForNewComment(string commenterUserName,string receiverUserName, string IssueId)
        {
            return $"Dear {receiverUserName},\n{commenterUserName} has commented in the Issue {IssueId}";
        }

        public string GetMailTemplateForNewIssue(string ownerUserName, string issuerUserName)
        {
            return $"Dear {ownerUserName},\n{issuerUserName} has created a new Issue";
        }

        public string GetMailTemplateForNewUser(string newRegisteredUser)
        {
            return $"Dear {newRegisteredUser},\nThank you for registering with us";
        }
    }
}

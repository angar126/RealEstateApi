using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApi.Models;

namespace RealEstateApi.Services
{
    public static class MailServiceExtensions
    {
        public static IServiceCollection AddMailService(this IServiceCollection services, IConfiguration configuration)
        {
            var mailConfig = new MailConfig();
            configuration.GetSection("EmailConfiguration").Bind(mailConfig);

            services.Configure<MailConfig>(op =>
            {
                op.Host = mailConfig.Host;
                op.Port = mailConfig.Port;
                op.Username = mailConfig.Username;
                op.Password = mailConfig.Password;
                op.Security = mailConfig.Security;
            });

            return services;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RealEstateApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApi.Models.Interfaces;
using RealEstateApi.Models.Templates;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.Repositories;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Repository;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RealEstateApi
{
    public class Startup
    {
        private readonly MailConfig _mailConfig;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserAirbnbDataContext>();
            services.AddDbContext<AirbnbDataContext>();
            services.AddMailService(Configuration);

            services.AddTransient<INotifier, MailNotification>();
            services.AddTransient<IMailTemplate, MailTemplates>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAutentication, AutenticationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IHouseRepository, HouseRepository>();
            services.AddTransient<IIssueRepository, IssueRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RealEstateApi", Version = "v1" });
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    ValidateIssuerSigningKey = true,

                     
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("CreateSomeRandomStringForSecretKey")),

                    
                    ValidateIssuer = false,

                    ValidateAudience = false
                };
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealEstateApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

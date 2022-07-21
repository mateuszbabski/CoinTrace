using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Identity.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtSettings = new JWTSettings();

            configuration.GetSection("Authentication").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();


            return services;

        }
    }
}
            



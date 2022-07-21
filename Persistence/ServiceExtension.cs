using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}

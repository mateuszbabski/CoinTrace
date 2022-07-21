using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Application.DTOs.Account;
using Application.DTOs.Budget;

namespace Application
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddScoped<IValidator<CreateBudgetRequest>, CreateBudgetValidator>();
            return services;
        }
    }
}

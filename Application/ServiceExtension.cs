using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Application.DTOs.Account;
using Application.DTOs.Budget;
using Application.Services;
using Application.Interfaces;
using Application.Middleware;

namespace Application
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlerMiddleware>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddScoped<IValidator<CreateBudgetRequest>, CreateBudgetValidator>();
            services.AddScoped<IBudgetService, BudgetService>();
            return services;
        }
    }
}

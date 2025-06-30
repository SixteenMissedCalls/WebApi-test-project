using System.Net.Http;
using CurrencyRateGateway.Application.Common.Interfaces;
using CurrencyRateGateway.Application.Pipelines;
using CurrencyRateGateway.Application.Queries.RateQuery;
using CurrencyRateGateway.Application.Services;
using CurrencyRateGateway.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

namespace CurrencyRateGateway.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddSwagger();
            services.AddCustomDependencies();
            services.AddMediatR();
            services.AddControllers()
                .AddApplicationPart(typeof(CurrencyRatesController).Assembly);
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Currency Rates Gateway API", 
                    Version = "v1",
                    Description = "API для получения курсов валют из Банка России"
                });

            });
        }

        private static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddHttpClient(); 
            services.AddLogging(loggingBuilder => 
            {
                loggingBuilder.AddSerilog(dispose: true);
            });
            services.AddScoped<IBankOfRussiaApiClient, BankOfRussiaApiClient>();
            services.AddScoped<IParseRatesAsync, ParseRatesAsync>();
        }

        private static void AddMediatR(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogFailuresBehavior<,>));  
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetRatesQueryHandler).Assembly));
        }
    }
}
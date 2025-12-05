using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.Services;

namespace SmartE_Commerce_Business
{
    public static class BusinessLogicExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {

            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}

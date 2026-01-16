using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartE_Commerce_Data.Contracts;
using SmartE_Commerce_Data.Repositories;

namespace SmartE_Commerce_Data
{
    public static class DataAccrssExtension
    {
        public static IServiceCollection AddService (this IServiceCollection services, IConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("con");
            services.AddDbContext<ECContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(typeof(IRepository<> ), typeof(Repository<>));
            services.AddScoped(typeof(IProductRepository ), typeof(ProductRepository));
            services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            return services;
        }

    }
}

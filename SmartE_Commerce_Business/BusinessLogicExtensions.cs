using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.Services;

namespace SmartE_Commerce_Business
{
    public static class BusinessLogicExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Register EmbeddingService with typed HttpClient
            var fastApiBaseUrl = configuration["FastAPI:BaseUrl"] ?? "http://localhost:8000";
            services.AddHttpClient<IEmbeddingService, EmbeddingService>(client =>
            {
                client.BaseAddress = new Uri(fastApiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(60); // Allow time for CLIP processing
            });

            return services;
        }
    }
}

using FinalProj.Data;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Data;
using SmartE_Commerce_Data.Contracts;
using SmartE_Commerce_Business;
namespace SmartE_Commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var con = builder.Configuration.GetConnectionString("con");
                
            builder.Services.AddSercice(builder.Configuration).AddBusinessLogicServices();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

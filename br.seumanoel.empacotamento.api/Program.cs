
using br.seumanoel.empacotamento.api.Factories;
using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Service;
using Microsoft.OpenApi.Models;

namespace br.seumanoel.empacotamento.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IBoxFactory, BoxFactory>();
            builder.Services.AddScoped<PackingService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Empacotamento API",
                    Version = "v1",
                    Description = "API para gerenciamento de caixas para empacotamento"
                })
            );

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

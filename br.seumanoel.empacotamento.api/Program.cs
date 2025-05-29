
using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Factories;
using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace br.seumanoel.empacotamento.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var key = Environment.GetEnvironmentVariable("JWT_KEY");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION"));
            });



            builder.Services.AddScoped<AuthService>();





            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            builder.Services.AddControllers();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<IBoxFactory, BoxFactory>();
            builder.Services.AddScoped<PackingService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Empacotamento API",
                    Version = "v1",
                    Description = "API para gerenciamento de caixas para empacotamento"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                           {
                               new OpenApiSecurityScheme
                               {
                                   Reference = new OpenApiReference
                                   {
                                       Type = ReferenceType.SecurityScheme,
                                       Id = "Bearer"
                                   }
                               },
                               new string[] {}
                           }
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

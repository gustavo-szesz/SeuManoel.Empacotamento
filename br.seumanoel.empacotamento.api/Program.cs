using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Factorie;
using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace br.seumanoel.empacotamento.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load env vars from .env file
            DotNetEnv.Env.Load();
            var key = Environment.GetEnvironmentVariable("JWT_KEY");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            var builder = WebApplication.CreateBuilder(args);

            #region  SQL Server 
            // add DbContext to the service 
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                // DB_CONNECTION is a envoriment var
                options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION"));
            });
            #endregion

            #region  Autentication JWT
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
            #endregion

            #region  Import Controllers and Services
            builder.Services.AddControllers();
            builder.Services.AddScoped<IBoxFactory, BoxFactory>();
            builder.Services.AddScoped<PackingService>();
            #endregion

            #region  Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Seu Manoel Empacotamento API - Teste técnico",
                    Version = "v1",
                    Description = "API Packing Seu Manoel",
                    Contact = new OpenApiContact
                    {
                        Name = "Gustavo Henrique Szesz",
                        Email = "szeszgutavo@gmail.com",
                    }
                });
                
                // add XML comments on Swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                
                // add JWT authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });
            #endregion

            var app = builder.Build();
            
            #region Seed Data, Migrations and Database Creation
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();

                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocorreu um erro ao aplicar migrações ou seed de dados.");
                }
            }
            #endregion

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

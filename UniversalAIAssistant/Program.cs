using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using UniversalAIAssistant.Application.Interfaces;
using UniversalAIAssistant.Application.Services;
using UniversalAIAssistant.Infrastructure.Persistence;

namespace UniversalAIAssistant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database Context (SQLite)
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // 2. HTTP Client for Ollama API calls
            builder.Services.AddHttpClient();

            // 3. Controllers (using System.Text.Json)
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            // 4. Swagger (OpenAPI) configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Universal Context AI Assistant API",
                    Version = "v1",
                    Description = "Multi-tenant RAG assistant using local LLM (Ollama) and vector search",
                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = "your.email@example.com"
                    }
                });

                // Optional: JWT authentication (commented – enable when needed)
                /*
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your token"
                });
                */
            });

            // 5. CORS policy (allow your frontend origins)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithOrigins(
                              "http://localhost:4200",
                              "https://localhost:4200",
                              "http://localhost:3000"
                          )
                          .AllowCredentials();
                });
            });

            // 6. Dependency Injection – AI Assistant core services
            //builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
            //builder.Services.AddScoped<ILLMService, LlmOllamaService>();
            //builder.Services.AddScoped<IChatService, ChatService>();
            //builder.Services.AddScoped<IContextService, ContextService>();
            //builder.Services.AddScoped<ITrainingService, TrainingService>();
            // VectorMemoryService is in-memory; can be Singleton for performance
            //builder.Services.AddSingleton<IVectorService, VectorMemoryService>();

            // Optional: HttpContextAccessor (if you need to access current user)
            builder.Services.AddHttpContextAccessor();

            // 7. Authentication (commented – enable when you add JWT)
            // builder.Services.AddAuthentication(...).AddJwtBearer(...);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Universal AI Assistant v1");
                    c.DocExpansion(DocExpansion.None);
                    c.DefaultModelsExpandDepth(-1);
                    c.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            // Uncomment when authentication is enabled
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}
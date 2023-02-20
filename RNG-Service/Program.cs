// RNG-Service - Program.cs
// Created on 2023.02.12
// Last modified at 2023.02.20 16:12

namespace RNG_Service;

public static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Set CORS Policy
        _ = builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                _ = policy.WithOrigins("https://*.netrve.net").SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials();
            });
        });

        // Add services to the container.
        _ = builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen();
        _ = builder.Services.AddApplicationInsightsTelemetry();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();
        _ = app.UseCors();

        _ = app.MapControllers();

        app.Run();
    }
}

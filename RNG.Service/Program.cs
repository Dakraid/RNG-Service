#region header
// RNG.Service
// RNG.Service / Program.cs BY Kristian Schlikow
// First modified on 2023.02.21
// Last modified on 2023.02.27
#endregion

namespace RNG.Service
{
    #region usings
    using Database;

    using Microsoft.AspNetCore.OData;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Fast.Components.FluentUI;
    using Microsoft.OData.Edm;
    using Microsoft.OData.ModelBuilder;

    using Models;
    using Syncfusion.Blazor;

    #endregion

    public class Program
    {
        private static void CreateDbIfNotExists(IHost app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DatabaseContext>();
                DatabaseInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred initializing the database.");
            }
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EntitySet<RngEntry>("RNG_List");
            builder.EntitySet<BatchedTest>("RNG_Tests");
            return builder.GetEdmModel();
        }

        private static void Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jSn5UdkBmWnpecX1VTg==;Mgo+DSMBPh8sVXJ0S0J+XE9HflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TdUVkWXpbc3FVRGBfWQ==;Mgo+DSMBMAY9C3t2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkZjXn9bdXdQRmJdVkw=;MTI3MDkyNkAzMjMwMmUzNDJlMzBMdnNVdDhnTStFTERORk5zYVJkT2xVSHNIaWlXNVM3R084TDdkcVowdEw4PQ==");

            var builder = WebApplication.CreateBuilder(args);

            // Add active controllers to the services
            builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Enable Application Insights Telemetry
            // builder.Services.AddApplicationInsightsTelemetry();

            // Enable Blazor server side web UI
            builder.Services.AddHttpClient();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddSyncfusionBlazor();
            
            // Set CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://*.netrve.net").SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials();
                });
            });

            // Build the web application
            var app = builder.Build();

            // Initialize the database if not present
            CreateDbIfNotExists(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Setup used app features
            app.UseHttpsRedirection();
            // app.UseCors();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Start the web app
            app.Run();
        }
    }
}

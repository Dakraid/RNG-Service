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

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Fast.Components.FluentUI;

    using Services;
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

        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add active controllers to the services
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enable Application Insights Telemetry
            // builder.Services.AddApplicationInsightsTelemetry();

            // Enable Blazor server side web UI
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor(o => o.DetailedErrors = true);

            // Setup Microsoft.Fast FluentUI component library
            builder.Services.AddHttpClient();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddDataGridEntityFrameworkAdapter();

            // Add services to the container.
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ResultsService>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Setup used app features
            app.UseHttpsRedirection();
            // app.UseCors();
            app.UseStaticFiles();
            app.UseRouting();

            // Map required elements
            app.MapControllers();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            // Start the web app
            app.Run();
        }
    }
}

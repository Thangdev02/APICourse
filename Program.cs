using Microsoft.EntityFrameworkCore;
using APICourse.Models; // Adjust with your actual namespace for ApplicationDbContext and Product
using System.Diagnostics;
using APICourse.Repositories; // Namespace của các repository

var builder = WebApplication.CreateBuilder(args);

// Add DbContext to services
builder.Services.AddDbContext<ApicourseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";
    });

    // Log to console to verify the environment
    Console.WriteLine("Application is running in Development mode.");

    // Automatically open the browser at Swagger UI
    var url = "http://localhost:5086/swagger"; // The URL to open
    Console.WriteLine($"Attempting to open browser at: {url}"); // Log URL

    OpenBrowser(url); // Function to open the browser
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

// Method to open the browser with the specified URL
void OpenBrowser(string url)
{
    try
    {
        Console.WriteLine($"Opening browser at {url}...");

        if (System.Environment.OSVersion.Platform == PlatformID.Win32NT) // Windows
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        else if (System.Environment.OSVersion.Platform == PlatformID.Unix) // macOS/Linux
        {
            // Check for macOS and Linux, use appropriate commands
            string command = (System.Environment.OSVersion.Platform == PlatformID.MacOSX) ? "open" : "xdg-open";
            Process.Start(new ProcessStartInfo(command, url) { UseShellExecute = true });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error opening browser: {ex.Message}");
    }
}

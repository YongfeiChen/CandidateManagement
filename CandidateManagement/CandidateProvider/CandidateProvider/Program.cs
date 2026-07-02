using CandidateProvider.Data;
using CandidateProvider.Mappings;
using CandidateProvider.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register database service
var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(typeof(Program).Assembly);
});

var app = builder.Build();

// Automatically execute database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration.");
    }
}

// Middleware pipeline configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS must be applied before MapControllers
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();

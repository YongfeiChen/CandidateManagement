using CandidateProvider.Data;
using CandidateProvider.Mappings;
using CandidateProvider.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. 注册数据库服务
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. 注册控制器和 OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 3. 配置 CORS (只保留一个)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 4. 配置 AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(typeof(Program).Assembly);
});

var app = builder.Build();

// 5. 自动执行数据库迁移 (保持不变，这很好)
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
        logger.LogError(ex, "执行数据库迁移时发生错误。");
    }
}

// 6. 中间件管道顺序调整
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 重要：UseCors 必须在 MapControllers 之前
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();

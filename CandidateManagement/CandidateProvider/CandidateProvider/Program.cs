using CandidateProvider.Data;
using CandidateProvider.Mappings;
using CandidateProvider.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// 注册数据库服务
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// 配置 CORS 以允许所有来源、方法和头部
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
// 注意这里的写法：参数名是 assemblies
// 注意这里的写法：参数名是 assemblies
// 显式引用你的 Profile 类名，确保它一定被加载
builder.Services.AddAutoMapper(config =>
{
    // 显式指定从哪个类所在的程序集里去找 MappingProfile
    config.AddMaps(typeof(Program).Assembly);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// 使用 CORS 策略
app.UseCors("AllowAll");
app.Run();

using AE_Backend.db;
using AE_Backend.General;
using AE_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AE API", Version = "v1" });
});

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("MyDbConnection")));

builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IRoleService, RoleServices>();
builder.Services.AddScoped<IShipService, ShipServices>();
builder.Services.AddScoped<IUserRoleService, UserRoleServices>();
builder.Services.AddScoped<IUserShipService, UserShipServices>();
builder.Services.AddScoped<IPortService, PortServices>();
builder.Services.AddScoped<Utility>();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AE API V1");
        c.RoutePrefix = ""; // Set the root URL to Swagger UI
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
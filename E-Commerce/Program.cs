using E_Commerce.BLL;
using E_Commerce.BLL.Interface;
using E_Commerce.Model;
using EmployeeDirectory.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEcommerce, EcommerceService>();
builder.Services.AddScoped<ICart, CartService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "master.sc.wdzkdv.euw2.cache.amazonaws.com:6379,abortConnect=false";
});


var customConfiguration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("CacheKey.json")
    .Build();
GetAllProductsKey.Datakey = customConfiguration.GetSection("GetProduct:Datakey").Value;
GetAllProductsKey.ExpiryTime = Convert.ToInt32(customConfiguration?.GetSection("GetProduct:ExpiryTime").Value);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.RequireHttpsMetadata = false;
        Options.SaveToken = true;
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

//Connection Service
Connection.ConnectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
Connection.LoggerTimeSpan = Convert.ToInt32(configuration.GetSection("Logger:TimeSpan").Value);

//For saving into file
Task fileLoggerTask = new Task(() =>
{
    while (true)
    {
        if (!CommonHelper.IsSaveInProgress)
        {
            CommonHelper.SaveQueuedDataIntoFile();
        }
        Thread.Sleep(Connection.LoggerTimeSpan);
    }
});
fileLoggerTask.Start();

// Logging settings
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Welcome, You are visiting API content. Please contact respective team for more information.");
});

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SocialApp.API.WebAPI.Models;
using SocialApp.API.WebAPI.Models.Entities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add logger
var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Add controllers to the container.
builder.Services.AddControllers();

// Add DB context
builder.Services.AddDbContext<SocialAppContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SocialAppContext")));

// Add automapper to map our entities to Dtos/ViewModels
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Inject identity stuff
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SocialAppContext>()
    .AddDefaultTokenProviders();

// Configure authentication, authorization
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
    });


// If you add a service that defines the logic then it should be injected here


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

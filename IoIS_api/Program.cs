using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SocialApp.API.WebAPI.Models;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services;
using System.Text;
using AutoMapper;
using Microsoft.OpenApi.Models;
using SocialApp.API.WebAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add logger
var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add controllers to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add DB context
if (Environment.GetEnvironmentVariable("ASPNETCORE_EVIRONMENT") == "Production")
{
    builder.Services.AddDbContext<SocialAppContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SocialAppContextProd")));
}
else
{
    builder.Services.AddDbContext<SocialAppContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SocialAppContext")));
}

// Add automapper to map our entities to Dtos/ViewModels
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add gRPC for communication
builder.Services.AddGrpc();

// Inject identity stuff
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SocialAppContext>()
    .AddDefaultTokenProviders();

// Configure authentication, authorization
builder.Services
    //  Adding authentication
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    //  Add JWt bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media Web API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IFriendService, FriendService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddScoped<UserManager<User>>();

// If you add a service that defines the logic then it should be injected here
builder.Services.AddTransient<IEventService, EventService>();

var app = builder.Build();

app.UseSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

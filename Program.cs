using BackEnd.Configuration;
using BackEnd.Mapper;
using BackEnd.Middleware;
using BackEnd.Repositories;
using BackEnd.Repositories.Advisor;
using BackEnd.Repositories.Event;
using BackEnd.Repositories.Lead;
using BackEnd.Services;
using BackEnd.Services.Advisor;
using BackEnd.Services.Event;
using BackEnd.Services.Lead;
using BackEnd.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using Typesense;
using Typesense.Setup;

var builder = WebApplication.CreateBuilder(args); 
var jwtSettings = builder.Configuration.GetSection("Jwt"); 
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 1️⃣ Add CORS service
builder.Services.AddCors(
    options => 
    { 
        options.AddPolicy(name: MyAllowSpecificOrigins, policy => { policy.WithOrigins("http://localhost:5173", "https://intercoracoid-tamiko-spasmodically.ngrok-free.dev") //front-end URL
                .AllowAnyHeader() 
                .AllowAnyMethod() 
                .AllowCredentials(); 
        }); 
    });

builder.Services
    .AddControllers(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new TimeOnlyNewtonsoftConverter());
    });

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(
    c => 
        { 
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            { 
                In = ParameterLocation.Header,
                Description = "Enter JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            }); 
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                                { 
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer" 
                                } 
                                }, 
                    new string[] {}
                } 
            });
        });

builder.Services.AddSwaggerGenNewtonsoftSupport(); 
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT Configuration
builder.Services.AddAuthentication(options => 
    { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
    })
    .AddJwtBearer(options => 
    { 
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true; 
        options.TokenValidationParameters = new TokenValidationParameters { 
                ValidateIssuer = true,
                ValidateAudience = true, 
                ValidateLifetime = true, 
                ValidateIssuerSigningKey = true, 
                ValidIssuer = jwtSettings["Issuer"], 
                ValidAudience = jwtSettings["Audience"], 
                IssuerSigningKey = new SymmetricSecurityKey(key) 
        }; 
    });

// Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IUserRepository, UserRepository>(); 
builder.Services.AddScoped<ILeadService, LeadService>(); 
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAdvisorService, AdvisorService>();
builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();
builder.Services.AddScoped<LeadMapper>();
builder.Services.AddScoped<EventMapper>();
builder.Services.AddScoped<AdvisorMapper>();
builder.Services.AddScoped<TypesenseService>();
// Validation 
builder.Services.AddValidatorsFromAssemblyContaining<LeadDtoValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<EventDtoValidation>();
builder.Services.AddFluentValidationAutoValidation();

// ****** Typesense *****************************************************

builder.Services.AddHttpClient<TypesenseService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8108");
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<TypesenseSettings>>().Value;

    var config = new Typesense.Setup.Config(
        new List<Node>
        {
            new Node(settings.Host, settings.Port, settings.Protocol)
        },
        settings.ApiKey
    );

    var httpClient = new HttpClient
    {
        BaseAddress = new Uri($"{settings.Protocol}://{settings.Host}:{settings.Port}")
    };

    return new TypesenseClient(
        Options.Create(config),
        httpClient
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment()) 
//     { 
        app.UseSwagger(); 
        app.UseSwaggerUI(); 
    // }

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication(); 
app.UseAuthorization();
// if (!app.Environment.IsDevelopment())
// {
//     app.UseHttpsRedirection();
// }
// app.UseAuthorization(); 
app.MapControllers(); 
app.Run();
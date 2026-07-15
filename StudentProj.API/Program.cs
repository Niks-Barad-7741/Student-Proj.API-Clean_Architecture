using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StudentProj.Application;
using StudentProj.Data;
using StudentProj.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
    
// Generate a unique log file name for this specific application run
string logFileName = $"Logs/log-{DateTime.Now:yyyyMMdd_HHmmss}.txt";

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    // Console gets ALL logs (including Microsoft internal logs)
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    // File ONLY gets custom logs (Microsoft internal logs are filtered out)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.MessageTemplate.Text.StartsWith("Name:"))
        .WriteTo.File(logFileName, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddApplicationDI();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                
                var response = StudentProj.DTO.ApiResponse<object>.Create(StudentProj.Domain.Enums.ResponseStatus.Unauthorized);
                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });

builder.Services.AddControllers()
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.Converters.Add(new StudentProj.API.Converters.TrimStringConverter());
    //})
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .Select(e => e.Value!.Errors.First().ErrorMessage)
                .ToList();

            var response = StudentProj.DTO.ApiResponse<object>.Create(
                StudentProj.Domain.Enums.ResponseStatus.InvalidData, 
                string.Join(" | ", errors));

            return new Microsoft.AspNetCore.Mvc.ObjectResult(response)
            {
                StatusCode = response.StatusCodes
            };
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "StudentProj API",
            Version = "v1"
        });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Autherization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token Like Bearer: your_token_here"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<StudentProj.API.Middleware.GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<StudentProj.API.Middleware.DynamicRbacMiddleware>();
app.MapControllers();

app.Run();


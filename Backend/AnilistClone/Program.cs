using System.Text;
using AnilistClone.AnilistClone.Data;
using AnilistClone.Exceptions;
using AnilistClone.Login;
using AnilistClone.Login.Interfaces;
using AnilistClone.Models;
using AnilistClone.Models.Enums;
using AnilistClone.Registration;
using AnilistClone.Registration.Interfaces;
using AnilistClone.Services;
using AnilistClone.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                ctx.Request.Cookies.TryGetValue("jwt_token", out var jwt_token);
                if (!string.IsNullOrEmpty(jwt_token))
                    ctx.Token = jwt_token;
                return Task.CompletedTask;
            },
        };
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding(
            (context, _) =>
            {
                var admin = context.Set<User>().FirstOrDefault(u => u.Username == "Admin");

                if (admin is null)
                {
                    var password = builder.Configuration.GetValue<string>("AdminPassword");
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        throw new InvalidOperationException("AdminPassword is missing or empty.");
                    }
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    var registeredAdmin = new User
                    {
                        Username = "Admin",
                        Password = hashedPassword,
                        UserType = UserType.Admin,
                    };

                    context.Set<User>().Add(registeredAdmin);
                    context.SaveChanges();
                }
            }
        )
        .UseAsyncSeeding(
            async (context, _, cancellationToken) =>
            {
                var admin = await context
                    .Set<User>()
                    .FirstOrDefaultAsync(u => u.Username == "Admin");

                if (admin is null)
                {
                    var password = builder.Configuration.GetValue<string>("AdminPassword");
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        throw new InvalidOperationException("AdminPassword is missing or empty.");
                    }
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    var registeredAdmin = new User
                    {
                        Username = "Admin",
                        Password = hashedPassword,
                        UserType = UserType.Admin,
                    };

                    context.Set<User>().Add(registeredAdmin);
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        )
);

builder.Services.AddHttpClient<IMediaService, MediaService>();
builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        bool connected = await db.Database.CanConnectAsync();

        Console.WriteLine(
            connected ? "Database connection successful!" : "Database connection failed!"
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection error:");
        Console.WriteLine(ex);
    }
}

app.MapHealthChecks("/health");

app.UseMiddleware<AnilistClone.Middleware.GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

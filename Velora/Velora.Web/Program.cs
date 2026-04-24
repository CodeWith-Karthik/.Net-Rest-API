using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Security.Claims;
using System.Text;
using Velora.Application;
using Velora.Infrastructure;
using Velora.Infrastructure.Data;
using Velora.Infrastructure.DbContexts;
using Velora.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastrutureServices();


// Add services to the container.
#region Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefautConnection"), x =>
    {
        x.EnableRetryOnFailure
        (
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null
        );
    });
});

#endregion

#region Auth

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});

#endregion

#region API Version

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ApiVersionReader = ApiVersionReader.Combine(
                            new QueryStringApiVersionReader("version"),
                            new UrlSegmentApiVersionReader(),
                            new HeaderApiVersionReader("X-API-Version"),
                            new MediaTypeApiVersionReader("version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion

#region Serilog

builder.Host.UseSerilog((context, config) =>
{
    string logFileName = "log.txt";

    var logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

    if (!Directory.Exists(logFolderPath))
    {
        Directory.CreateDirectory(logFolderPath);
    }

    var logFilePath = Path.Combine(logFolderPath, logFileName);
    config.WriteTo.File(Path.Combine(logFilePath), rollingInterval: RollingInterval.Day);
    config.WriteTo.Console();
});

#endregion

builder.Services.AddResponseCaching();

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("1Min", new CacheProfile
    {
        Duration = 60
    });
    options.CacheProfiles.Add("2Min", new CacheProfile
    {
        Duration = 120
    });
}).AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Velora API version 1",
        Description = "Developed By Karthik",
        Version = "v1.0"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Velora API version 2",
        Description = "Developed By Karthik",
        Version = "v2.0"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter Bearer Token"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

var app = builder.Build();

#region Data Seeding

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsNpgsql())
        {
            var database = (IRelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();

            if (!database.Exists())
            {
                context.Database.Migrate();
            }
        }

        await SeedData.SeedRolesAsync(services);

        await SeedData.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    var versions = app.DescribeApiVersions();

    foreach (var version in versions)
    {
        var url = $"/swagger/{version.GroupName}/swagger.json";
        var name = version.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name);
    }
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

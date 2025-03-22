using System.Reflection;
using System.Text;
using System.Text.Json;
using Currencies.Abstractions.Contracts;
using Currencies.Application.Interfaces;
using Currencies.Application.Middleware;
using Currencies.Application.Services;
using Currencies.Db;
using Currencies.Db.Entities;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;

namespace Currencies.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var envConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        builder.Configuration.AddConfiguration(envConfig);

        builder.Services.AddCors(config =>
        {
            config.AddPolicy("CurrenciesCorsPolicy", policy => policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AEH Currencies 2025 API", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
        
        builder.Services.AddDbContext<TableContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("Default"),
                x => x.MigrationsAssembly("Currencies.Migrations")));

        DatabaseManager databaseManager = new(builder);

        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserCurrencyAmountService, UserCurrencyAmountService>();
        builder.Services.AddScoped<IUserExchangeHistoryService, UserExchangeHistoryService>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<ErrorHandlingMiddleware>();

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<TableContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.SaveToken = true;
            config.RequireHttpsMetadata = false;
            config.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Query.ContainsKey("accessToken"))
                    {
                        context.Token = context.Request.Query["accessToken"];
                    }

                    return Task.CompletedTask;
                }
            };

            config.Events = new JwtBearerEvents()
            {
                OnChallenge = async context =>
                {
                    context.Error = "Invalid JWT access token.";

                    context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                    var response = new BaseResponse<BaseResponseError>
                    {
                        ResponseCode = StatusCodes.Status401Unauthorized,
                        Message = "Authorization failed.",
                        BaseResponseError = new List<BaseResponseError>
                        {
                            new BaseResponseError(
                                propertyName: context.Request.Path,
                                message: context.ErrorDescription,
                                code: context.Error)
                        }
                    };
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    await context.Response.CompleteAsync();
                }
            };

            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"] ?? throw new InvalidOperationException())),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            var defaultPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

            defaultPolicyBuilder = defaultPolicyBuilder.RequireAuthenticatedUser();

            options.DefaultPolicy = defaultPolicyBuilder.Build();
        });

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        
        var app = builder.Build();
        databaseManager.ApplyMigrations(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AEH Currencies 2024 API v1"); });
        }

        app.UseRouting();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseCors("CurrenciesCorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
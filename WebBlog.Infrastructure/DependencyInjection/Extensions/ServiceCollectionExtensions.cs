using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WebBlog.Application.Abstraction;
using WebBlog.Application.ExternalServices;
using WebBlog.Application.Interfaces;
using WebBlog.Application.Services;
using WebBlog.Infrastructure.ExternalServices;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Mappings;
using WebBlog.Infrastructure.Persistances;
using WebBlog.Infrastructure.Services;
using WebBlog.Infrastructure.Workers;
namespace WebBlog.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlServerPersistence(this IServiceCollection services)
        {
            services.AddDbContextPool<DbContext, AppDbContext>((provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

                builder.EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(true)  // => If UseLazyLoadingProxies, all of the navigation field should be VIRTUAL
                .UseSqlServer(

                    connectionString: configuration.GetConnectionString("ConnectionString"),
                    sqlServerOptionsAction: optionsBuider
                                    => optionsBuider.ExecutionStrategy(
                                                    dependencies => new SqlServerRetryingExecutionStrategy(
                                                        dependencies: dependencies,
                                                        maxRetryCount: options.CurrentValue.MaxRetryCount,
                                                        maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                                        errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                                    .MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name))
                .AddInterceptors();
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            
         }

        public static void AddRepositoryPersistence(this IServiceCollection services)
        {
            services.AddScoped<IAppDBRepository, AppDBRepository<AppDbContext>>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

        public static void AddBackgroupTaskQueue(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<BackgroundWorkerService>();
        }

        public static void AddInterceptorPersistence(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptionsPersistence(this IServiceCollection services, IConfigurationSection section)
        {
             return services.AddOptions<SqlServerRetryOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        public static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = configuration["Tokens:Issuer"],
                    //ValidAudience = configuration["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse(); // Prevents the default 401 response
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { message = "Unauthorized access. Please provide a valid token." });
                        return context.Response.WriteAsync(result);
                    }
                };
            });           
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });
                // To Enable authorization using Swagger (JWT)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ8\"",
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
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Cache:Configuration"];
                options.InstanceName = configuration["Cache:Instancename"];
            });

            services.AddScoped<ICacheService, CacheService>();
        }
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
            
            builder.Services.AddScoped<IAppLogger, AppLogger>();
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {           
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        }
    }
}

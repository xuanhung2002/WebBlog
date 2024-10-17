using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Mapper;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlServerPersistence(this IServiceCollection services)
        {
            services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
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
                                    .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .AddInterceptors();
            });

            services.AddIdentityCore<AppUser>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
         }

        public static void AddRepositoryPersistence(this IServiceCollection services)
        {
            ///
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapper = AutoMapperConfiguration.RegisterMap().CreateMapper();
            services.AddSingleton(mapper);
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
            });
            
        }
    }
}

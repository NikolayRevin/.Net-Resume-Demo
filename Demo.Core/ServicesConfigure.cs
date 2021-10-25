using Demo.Core.DAL;
using Demo.Core.DAL.Entities;
using Demo.Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Demo.Core.Interfaces;
using Demo.Core.Services;
using Hangfire;
using System.Transactions;
using System;
using Hangfire.MySql;

namespace Demo.Core
{
    public static class ServicesConfigure
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var commonSettingsSection = configuration.GetSection(CommonSettings.SectionName);
            var jwtSettingsSection = configuration.GetSection(JwtSettings.SectionName);
            var commonSettings = commonSettingsSection.Get<CommonSettings>();
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

            services
                .Configure<CommonSettings>(commonSettingsSection)
                .Configure<JwtSettings>(jwtSettingsSection)
                .Configure<MailSettings>(configuration.GetSection(MailSettings.SectionName));

            var sqlConnectionString = commonSettings.SqlConnectionString;
            services.AddDbContext<DbManager>(
                options => options.UseMySql(
                    sqlConnectionString,
                    ServerVersion.AutoDetect(sqlConnectionString)
                )
            );

            services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<DbManager>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

            services
                .AddSingleton<IMailService, MailService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IAuthService, AuthService>();

            services.AddHangfire(options =>
            {
                options.UseStorage(new MySqlStorage(sqlConnectionString, new MySqlStorageOptions()
                {
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                    TablesPrefix = "Hangfire"
                }));
            });

            services.AddHangfireServer();
        }
    }
}

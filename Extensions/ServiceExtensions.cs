using AudioArchive.Shared;
using AudioArchive.Database;
using AudioArchive.Infrastructure.Caching;
using AudioArchive.Infrastructure.Identity;
using AudioArchive.Infrastructure.Settings;
using AudioArchive.Infrastructure.Providers;

using AudioArchive.Modules.Core.Services;
using AudioArchive.Modules.Tags.Services;
using AudioArchive.Modules.Audios.Services;
using AudioArchive.Modules.Artists.Services;
using AudioArchive.Modules.Playlists.Services;
using AudioArchive.Modules.Support.Services;

using System.Text;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AudioArchive.Extensions
{
  public static class ServiceExtensions
  {
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config) {
      services.AddDbContext<DatabaseContext>(options =>
          options.UseNpgsql(config.GetConnectionString("Postgres")));
      return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config) {
      services.AddSingleton<IConnectionMultiplexer>(_ => {
        var connectionString = config.GetConnectionString("Redis")
            ?? throw new MissingFieldException("Redis connection string not found.");
        return ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(connectionString));
      });
      return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
      services.AddExceptionHandler<GlobalExceptionHandler>();
      services.AddHostedService<TagCleanupService>();
      services.AddScoped<ITagService, TagService>();
      services.AddScoped<IAudioService, AudioService>();
      services.AddScoped<IArtistService, ArtistService>();
      services.AddScoped<IPlaylistService, PlaylistService>();
      services.AddSingleton<ICachingService, CachingService>();
      services.AddScoped<IAccountService, AccountService>();
      services.AddScoped<ISupportService, SupportService>();
      services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
      services.AddTransient<IEmailSender, EmailProvider>();
      return services;
    }

    public static IServiceCollection AddApplicationMiddlewares(this IServiceCollection services) {
      services.AddTransient<CachingMiddleware>();
      return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services) {
      services.AddCors(options =>
          options.AddPolicy("AllowAll", policy =>
              policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
      return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration) {
      var jwt = configuration.GetSection("JwtSettings").Get<JwtSettings>()
        ?? throw new InvalidOperationException("JwtSettings not configured.");

      services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
          options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(jwt.SigningKey)
            )
          };
        });

      services.AddScoped<ICurrentAccount, CurrentAccount>();
      services.AddScoped<IAuthorizationHandler, PermissionHandler>();

      services.AddAuthorization(options => {
        options.AddPolicy("audio:write", p => p.AddRequirements(new PermissionRequirement("audio:write")));
        options.AddPolicy("audio:delete", p => p.AddRequirements(new PermissionRequirement("audio:delete")));
        options.AddPolicy("audio:update", p => p.AddRequirements(new PermissionRequirement("audio:update")));

        options.AddPolicy("account:read", p => p.AddRequirements(new PermissionRequirement("account:read")));
        options.AddPolicy("account:write", p => p.AddRequirements(new PermissionRequirement("account:write")));
        options.AddPolicy("account:update", p => p.AddRequirements(new PermissionRequirement("account:update")));
        options.AddPolicy("account:delete", p => p.AddRequirements(new PermissionRequirement("account:delete")));
      });

      return services;
    }
  }
}
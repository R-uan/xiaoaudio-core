using AudioArchive.Shared;
using AudioArchive.Database;
using AudioArchive.Infrastructure.Caching;
using AudioArchive.Infrastructure.Providers;

using AudioArchive.Modules.Core.Services;
using AudioArchive.Modules.Tags.Services;
using AudioArchive.Modules.Audios.Services;
using AudioArchive.Modules.Artists.Services;
using AudioArchive.Modules.Support.Services;

using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

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
      services.AddHostedService<TagCleanupService>();
      services.AddScoped<IAudioService, AudioService>();
      services.AddScoped<IArtistService, ArtistService>();
      services.AddSingleton<ICachingService, CachingService>();
      services.AddExceptionHandler<GlobalExceptionHandler>();
      services.AddScoped<IAccountService, AccountService>();
      services.AddScoped<ISupportService, SupportService>();
      services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
      services.AddScoped<IEmailSender, EmailProvider>();
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
  }
}
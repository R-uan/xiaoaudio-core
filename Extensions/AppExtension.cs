using AudioArchive.Database;
using AudioArchive.Database.Entity;
using AudioArchive.Infrastructure.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace AudioArchive.Extensions
{
  public static class AppExtensions
  {
    public static WebApplication UseAudioStaticFiles(this WebApplication app, string staticFilesPath) {
      var contentTypeProvider = new FileExtensionContentTypeProvider();
      contentTypeProvider.Mappings[".mp3"] = "audio/mpeg";
      contentTypeProvider.Mappings[".flac"] = "audio/flac";
      contentTypeProvider.Mappings[".wav"] = "audio/wav";
      contentTypeProvider.Mappings[".ogg"] = "audio/ogg";
      contentTypeProvider.Mappings[".m4a"] = "audio/mp4";

      var fileProvider = new PhysicalFileProvider(staticFilesPath);
      app.UseStaticFiles(new StaticFileOptions {
        FileProvider = fileProvider,
        RequestPath = "/media/audio",
        ContentTypeProvider = contentTypeProvider,
      });
      app.UseDirectoryBrowser(new DirectoryBrowserOptions {
        FileProvider = fileProvider,
        RequestPath = "/media/audio"
      });
      return app;
    }

    public static WebApplication MigrateDatabase(this WebApplication app) {
      using var scope = app.Services.CreateScope();
      scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
      return app;
    }

    public static async Task<WebApplication> CreateAdminAccountAsync(this WebApplication app) {
      using var scope = app.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
      var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

      var settings = config.GetSection("AdministratorAccount").Get<AdminAccountSettings>()
        ?? throw new InvalidOperationException("AdministratorAccount not configured.");

      var exists = await db.Accounts.AnyAsync(a => a.Username == settings.Username);
      if (exists) return app;

      var permissions = await db.Permissions.ToListAsync();
      if (!permissions.Any()) {
        permissions = [
          new Permission { Name = "audio:write" },
          new Permission { Name = "audio:delete" },
          new Permission { Name = "audio:update" },

          new Permission { Name = "account:read" },
          new Permission { Name = "account:write" },
          new Permission { Name = "account:update" },
          new Permission { Name = "account:delete" },
        ];
        db.Permissions.AddRange(permissions);
      }

      var admin = new Account {
        Email = settings.Email,
        Username = settings.Username,
        Password = settings.Password,
        Permissions = permissions
      };

      db.Accounts.Add(admin);
      await db.SaveChangesAsync();

      return app;
    }
  }
}
using AudioArchive.Database;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
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
  }
}
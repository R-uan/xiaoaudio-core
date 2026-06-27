using System.Data;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Audios.Requests;
using AudioArchive.Modules.Audios.Responses;

namespace AudioArchive.Modules.Audios.Services
{
  public partial class AudioService
  {
    public async Task<List<Audio>> AudioQueryAsync(AudioSearchParams parameters) {
      var query = _db.Audios
        .Include(a => a.Artist)
        .Include(a => a.Metadata)
          .ThenInclude(m => m.Tags)
        .AsQueryable();

      if (!string.IsNullOrEmpty(parameters.Artist))
        query = query.Where(a => EF.Functions.ILike(a.Artist.Name, $"%{parameters.Artist}%"));

      if (!string.IsNullOrEmpty(parameters.Title))
        query = query.Where(a => EF.Functions.ILike(a.Title, $"%{parameters.Title}%"));

      if (!string.IsNullOrEmpty(parameters.IncludeTags)) {
        foreach (var tag in parameters.IncludeTags.Split(",")) {
          var captured = tag;
          query = query.Where(a => a.Metadata.Tags.Any(t => t.Name == captured));
        }
      }

      if (!string.IsNullOrEmpty(parameters.ExcludeTags)) {
        foreach (var tag in parameters.ExcludeTags.Split(",")) {
          var captured = tag;
          query = query.Where(a => !a.Metadata.Tags.Any(t => t.Name == captured));
        }
      }

      if (parameters.MinDuration > 0)
        query = query.Where(a => a.Metadata.Duration >= parameters.MinDuration);

      if (parameters.MaxDuration > 0)
        query = query.Where(a => a.Metadata.Duration <= parameters.MaxDuration);

      return await query.ToListAsync();
    }
  }
}

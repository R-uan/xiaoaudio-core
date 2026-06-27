using AudioArchive.Database.Entity;
using AudioArchive.Modules.Playlists.Requests;
using AudioArchive.Modules.Playlists.Responses;
using AudioArchive.Shared;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Playlists.Services
{
  public partial class PlaylistService
  {
    public async Task<PlaylistResponse> CreatePlaylistAsync(CreatePlaylistRequest request)
    {
      var playlist = Playlist.FromRequest(request);
      if (request.Audios != null && request.Audios.Count != 0)
      {
        var validIds = request.Audios.Where(a => a != Guid.Empty);
        var existingAudios = await _db.Audios.Where(a => validIds.Contains(a.Id)).ToListAsync();
        playlist.Audios = existingAudios;
      }

      var save = await _db.Playlists.AddAsync(playlist);
      await _db.SaveChangesAsync();

      return new PlaylistResponse
      {
        Id = save.Entity.Id,
        Name = save.Entity.Name,
        CreatedAt = save.Entity.CreatedAt,
        Audios = save.Entity.Audios?.Select(a => a.Id).ToList(),
      };
    }

    public async Task DeletePlaylistAsync(Guid playlistId)
    {
      var playlist = await _db.Playlists.FindAsync(playlistId)
        ?? throw new NotFoundException(
          Message: $"Playlist entry was not found.",
          Target: playlistId.ToString()
        );

      _db.Playlists.Remove(playlist);
      await _db.SaveChangesAsync();
    }

    public async Task<PlaylistResponse> UpdatePlaylistAsync(Guid playlistId, PatchPlaylistRequest request)
    {
      var playlist = await _db.Playlists
        .Include(p => p.Audios)
        .FirstOrDefaultAsync(p => p.Id == playlistId)
        ?? throw new NotFoundException(
          Message: $"Playlist entry was not found.",
          Target: playlistId.ToString()
        );

      if (!string.IsNullOrEmpty(request.Name))
        playlist.Name = request.Name;

      if (request.RemoveAudios?.Count > 0)
      {
        var validAudioIds = request.RemoveAudios.Where(a => a != Guid.Empty);
        playlist.Audios?.RemoveAll(a => validAudioIds.Contains(a.Id));
      }

      if (request.AddAudios?.Count > 0)
      {
        var validAudioIds = request.AddAudios.Where(a => a != Guid.Empty);
        var existingAudios = await _db.Audios.Where(a => validAudioIds.Contains(a.Id)).ToListAsync();
        (playlist.Audios ??= []).AddRange(existingAudios);
      }

      await _db.SaveChangesAsync();

      return new PlaylistResponse
      {
        Id = playlist.Id,
        Name = playlist.Name,
        CreatedAt = playlist.CreatedAt,
        Audios = playlist.Audios?.Select(a => a.Id).ToList(),
      };
    }
  }
}

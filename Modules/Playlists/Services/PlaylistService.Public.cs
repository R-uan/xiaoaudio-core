using AudioArchive.Modules.Playlists.Responses;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Playlists.Services
{
  public partial class PlaylistService
  {
    public async Task<List<PlaylistResponse>> GetPlaylistsAsync()
    {
      return await _db.Playlists
        .Include(p => p.Audios)
        .Select(p => new PlaylistResponse
        {
          Id = p.Id,
          Name = p.Name,
          CreatedAt = p.CreatedAt,
          Audios = p.Audios != null ? p.Audios.Select(a => a.Id).ToList() : null,
        })
        .ToListAsync();
    }

    public async Task<PlaylistResponse?> GetPlaylistAsync(Guid playlistId)
    {
      return await _db.Playlists
        .Include(p => p.Audios)
        .Select(p => new PlaylistResponse
        {
          Id = p.Id,
          Name = p.Name,
          CreatedAt = p.CreatedAt,
          Audios = p.Audios != null ? p.Audios.Select(a => a.Id).ToList() : null,
        })
        .FirstOrDefaultAsync(p => p.Id == playlistId);
    }
  }
}

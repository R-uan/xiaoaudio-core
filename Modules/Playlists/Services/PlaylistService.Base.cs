using AudioArchive.Database;
using AudioArchive.Database.Entity;
using AudioArchive.Modules.Playlists.Requests;
using AudioArchive.Modules.Playlists.Responses;

namespace AudioArchive.Modules.Playlists.Services
{
  public interface IPlaylistService
  {
    // Public
    Task<List<PlaylistResponse>> GetPlaylistsAsync();
    Task<PlaylistResponse?> GetPlaylistAsync(Guid playlistId);
    // Internal
    Task<PlaylistResponse> CreatePlaylistAsync(CreatePlaylistRequest request);
    Task DeletePlaylistAsync(Guid playlistId);
    Task<PlaylistResponse> UpdatePlaylistAsync(Guid playlistId, PatchPlaylistRequest request);
  }

  public partial class PlaylistService(DatabaseContext databaseContext) : IPlaylistService
  {
    private readonly DatabaseContext _db = databaseContext;
  }
}

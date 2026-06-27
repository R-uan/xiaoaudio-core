using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Playlists.Services;
using AudioArchive.Infrastructure.Identity;

namespace AudioArchive.Modules.Playlists.Controllers
{
  [ApiController]
  [Route("api/playlist")]
  public partial class PlaylistController(
    IPlaylistService playlistService,
    ICurrentAccount currentAccout
  ) : ControllerBase
  {
    private readonly IPlaylistService _playlistService = playlistService;
    private readonly ICurrentAccount _currentAccount = currentAccout;
  }
}

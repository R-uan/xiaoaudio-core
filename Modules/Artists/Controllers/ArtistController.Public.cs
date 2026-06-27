using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Artists.Controllers
{
  public partial class ArtistController : ControllerBase
  {
    [HttpGet]
    public async Task<IActionResult> GetArtists() {
      var artists = await _cachingService.GetAsync(CacheGroup, "all", () => {
        return _databaseContext.Artists.Include(a => a.Audios).ToListAsync();
      });

      return Ok(new {
        Count = artists == null ? 0 : artists.Count,
        Data = artists?.Select(a => new {
          a.Id,
          a.Name,
          a.Note,
          AudioCount = a.Audios == null ? 0 : a.Audios.Count
        }).ToList().OrderBy(a => a.Name)
      });
    }

    [HttpGet("profiles")]
    public async Task<IActionResult> GetProfiles() {
      var profiles = await _cachingService.GetAsync(CacheGroup, "profiles", () => {
        return _artistService.ArtistProfilesAsync();
      });

      return Ok(profiles);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetArtistByName([FromRoute] string name) {
      return Ok(await _artistService.ArtistProfileByNameAsync(name));
    }
  }
}

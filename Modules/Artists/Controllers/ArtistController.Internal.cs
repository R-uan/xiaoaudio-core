using AudioArchive.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AudioArchive.Modules.Artists.Requests;

namespace AudioArchive.Modules.Artists.Controllers
{
  public partial class ArtistController : ControllerBase
  {
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostArtist([FromBody] PostArtistRequest body) {
      return Ok(await _artistService.InsertArtistAsync(body));
    }

    [HttpPatch("{artistId}")]
    [Authorize]
    public async Task<IActionResult> PatchArtist([FromRoute] string artistId, [FromBody] PatchArtistRequest body) {
      if (!Guid.TryParse(artistId, out var artistGuid)) {
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: artistId
        );
      }
      return Ok(await _artistService.UpdateArtistAsync(artistGuid, body));
    }

    [HttpDelete("{artistId}")]
    [Authorize]
    public async Task<IActionResult> DeleteArtist([FromRoute] string artistId) {
      if (!Guid.TryParse(artistId, out var artistGuid)) {
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: artistId
        );
      }

      var artist = await _databaseContext.Artists.FindAsync(artistGuid) ??
        throw new NotFoundException(
          Message: "Could not find artist entry.",
          Target: artistId
        );

      _databaseContext.Remove(artist);
      await _databaseContext.SaveChangesAsync();

      return base.Ok(new {
        Message = "Artist successfully deleted.",
        Target = artistId
      });
    }
  }
}

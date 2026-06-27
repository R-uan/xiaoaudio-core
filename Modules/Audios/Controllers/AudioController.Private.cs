using AudioArchive.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace AudioArchive.Modules.Audios.Controllers
{
  public partial class AudioController : ControllerBase
  {
    [Authorize]
    [HttpGet("favourite/{audioId}")]
    public async Task<IActionResult> ToggleFavouriteAsync(string audioId) {
      if (!Guid.TryParse(audioId, out var audioGuid)) {
        throw new BadRequestException(
          Message: "Invalid audio id given.",
          Target: "AudioController::FavouriteAudioAsync"
        );
      }
      
      return Ok(new { 
        Success = true,
        Data = new { 
          Favourited = await this._audioService.ToggleFavouriteAudioAsync(audioGuid) 
        }
      });
    }
    
    [Authorize]
    [HttpGet("favourite")]
    public async Task<IActionResult> GetFavouritesAsync() {
      return Ok(new {
        Success = true,
        Data = await this._audioService.ListFavouritesAync()
      });
    }
  }
}

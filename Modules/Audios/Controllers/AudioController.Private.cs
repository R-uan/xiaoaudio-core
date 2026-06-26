using AudioArchive.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace AudioArchive.Modules.Audios.Controllers
{
  public partial class AudioController : ControllerBase
  {
    [Authorize]
    [HttpGet("favourite/{audioId}")]
    public async Task<IActionResult> FavouriteAudioAsync(string audioId) {
      if (!Guid.TryParse(audioId, out var audioGuid)) {
        throw new BadRequestException(
          Message: "Invalid audio id given.",
          Target: "AudioController::FavouriteAudioAsync"
        );
      }
      
      return Ok(new { 
        Success = await this._audioService.FavouriteAudioAsync(audioGuid) 
      });
    }
  }
}

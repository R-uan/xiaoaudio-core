using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AudioArchive.Modules.Audios.Requests;


namespace AudioArchive.Modules.Audios.Controllers
{
  public partial class AudioController : ControllerBase
  {
    [HttpPost]
    [Authorize(Policy = "audio:write")]
    public async Task<IActionResult> PostAudio([FromBody] PostAudioRequest req) {
      return this.Ok(new {
        Success = true,
        Data = await this._audioService.InsertAudioAsync(req)
      });
    }
  }
}

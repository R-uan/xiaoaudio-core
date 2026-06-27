using AudioArchive.Shared;
using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Audios.Requests;
using AudioArchive.Modules.Audios.Responses;

using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Audios.Controllers
{
  public partial class AudioController : ControllerBase
  {
    [HttpGet]
    public async Task<IActionResult> GetAudios() {
      var audios = await _cachingService.GetAsync(CacheGroup, "all", () => {
        return _databaseContext.Audios
              .Include(a => a.Metadata)
              .Select(audio => new AudioResponse {
                Id = audio.Id,
                Title = audio.Title,
                Artist = audio.Artist.Name,
                Source = audio.Source,
                Link = audio.Link,
                Local = audio.Local,
                AddedAt = audio.AddedAt,
                UpdatedAt = audio.UpdatedAt,
                Metadata = new MetadataResponse {
                  Duration = audio.Metadata.Duration,
                  Genre = audio.Metadata.Genre,
                  ReleaseYear = audio.Metadata.ReleaseYear,
                  Tags = audio.Metadata.Tags.Select(t => t.Name).Order().ToList(),
                }
              }).OrderBy(a => a.Title).ToListAsync();
      });

      return Ok(new {
        Data = audios,
        Count = audios == null ? 0 : audios.Count,
        AudiosOverallDuration = audios?.Sum(a => a.Metadata.Duration),
      });
    }

    [HttpGet("{audioId}")]
    public async Task<IActionResult> GetAudio([FromRoute] string audioId) {
      if (Guid.TryParse(audioId, out var audioGuid))
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: audioId
        );

      var audio = await _databaseContext.Audios.Include(a => a.Artist)
        .Include(a => a.Metadata).ThenInclude(m => m.Tags)
        .Where(a => a.Id == audioGuid).FirstOrDefaultAsync() ??
          throw new NotFoundException(
            Message: "Could not find audio entry.",
            Target: audioId
          );

      return base.Ok(AudioResponse.From(audio));
    }

    [HttpGet("q")]
    public async Task<IActionResult> QueryAudios([FromQuery] AudioSearchParams parameters) {
      var key = HttpContext.Request.QueryString.ToString();
      var audios = (await _cachingService.GetAsync(CacheGroup, key, () => {
        return _audioService.AudioQueryAsync(parameters);
      }))?.Select(AudioResponse.From).ToList();

      return base.Ok(new {
        Data = audios,
        Count = audios == null ? 0 : audios.Count
      });
    }
  }
}

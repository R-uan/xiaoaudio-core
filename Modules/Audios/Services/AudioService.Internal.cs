using System.Data;
using AudioArchive.Database.Entity;
using AudioArchive.Modules.Audios.Requests;
using AudioArchive.Modules.Audios.Responses;

using AudioArchive.Shared;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Audios.Services
{
  public partial class AudioService
  {
    public async Task<PostAudioResponse> InsertAudioAsync(PostAudioRequest req) {
      var artist = await _db.Artists
          .Include(a => a.Audios)
          .Where(a => a.Name == req.Artist)
          .FirstOrDefaultAsync();

      if (artist == null) {
        artist = new Artist {
          Id = Guid.NewGuid(),
          Name = req.Artist,
          InActivity = false
        };
      }

      var audio = Audio.FromRequest(req, artist);
      bool isNew = true;

      if (artist.Audios != null && artist.Audios.Any(a => a.Source.Contains(audio.Source))) {
        audio = await _db.Audios
          .Include(a => a.Metadata)
          .ThenInclude(m => m.Tags)
          .Where(a => a.Source == req.Source)
          .FirstAsync();
        isNew = false;
      }

      if (req.Tags != null) {
        var audioTags = await ProcessTagsAsync(req.Tags);

        audio.Metadata.Tags ??= [];

        if (!isNew) {
          var existingTagIds = audio.Metadata.Tags.Select(t => t.Id).ToHashSet();
          audioTags = audioTags.Where(t => !existingTagIds.Contains(t.Id)).ToList();
          audio.UpdatedAt = DateTime.UtcNow;
        }

        audio.Metadata.Tags.AddRange(audioTags);
      }

      if (isNew) {
        await _db.Audios.AddAsync(audio);
      }

      await _db.SaveChangesAsync();
      return new PostAudioResponse {
        Inserted = isNew,
        Audio = AudioResponse.From(audio),
      };
    }

    public async Task<Audio> UpdateAudioAsync(Guid audioId, PatchAudioRequest req) {
      var audio = await _db.Audios
        .Include(a => a.Metadata)
          .ThenInclude(m => m.Tags)
        .Include(a => a.Artist)
        .Where(a => a.Id == audioId)
        .FirstOrDefaultAsync() ??
          throw new NotFoundException(
            Message: "Could not find audio entry.",
            Target: audioId.ToString()
          );

      if (!string.IsNullOrEmpty(req.Title)) audio.Title = req.Title.Trim();
      if (!string.IsNullOrEmpty(req.Link)) audio.Link = req.Link;
      if (!string.IsNullOrEmpty(req.Source)) audio.Source = req.Source;
      if (req.Local.HasValue) audio.Local = req.Local.Value;

      if (!string.IsNullOrEmpty(req.Artist)) {
        var artist = await _db.Artists
          .Where(a => a.Name == req.Artist)
          .FirstOrDefaultAsync();

        if (artist == null) {
          artist = new Artist {
            Id = Guid.NewGuid(),
            Name = req.Artist,
            InActivity = true
          };

          await _db.Artists.AddAsync(artist);
        }

        audio.Artist = artist;
        audio.ArtistId = artist.Id;
      }

      if (req.Duration.HasValue) audio.Metadata.Duration = req.Duration.Value;
      if (!string.IsNullOrEmpty(req.Genre)) audio.Metadata.Genre = req.Genre.Trim();
      if (req.ReleaseYear.HasValue) audio.Metadata.ReleaseYear = req.ReleaseYear.Value;

      if (req.AddTags != null && req.AddTags.Count > 0) {
        var tags = await ProcessTagsAsync(req.AddTags);
        (audio.Metadata.Tags ??= []).AddRange(tags);
      }

      if (req.RemoveTags != null && req.RemoveTags.Count > 0) {
        audio.Metadata.Tags?.RemoveAll(t => req.RemoveTags.Contains(t.Name));
      }

      audio.UpdatedAt = DateTime.UtcNow;
      await _db.SaveChangesAsync();
      return audio;
    }

    private async Task<List<Tag>> ProcessTagsAsync(List<string> tags) {
      var lowerTargetTags = tags.Select(t => t.ToLower().Trim()).ToList();
      var existingTags = await _db.Tags.Where(t => lowerTargetTags.Contains(t.Name.ToLower()))
        .ToListAsync();
      var newTags = lowerTargetTags.Where(t => !existingTags.Exists(tag => tag.Name.ToLower() == t))
        .Select(t => new Tag(t)).ToList();

      if (newTags.Count != 0) {
        await _db.Tags.AddRangeAsync(newTags);
        await _db.SaveChangesAsync();
        existingTags.AddRange(newTags);
      }

      return existingTags;
    }
  }
}

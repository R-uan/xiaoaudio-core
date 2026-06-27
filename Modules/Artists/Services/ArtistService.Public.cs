using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Artists.Responses;

namespace AudioArchive.Modules.Artists.Services
{
  public partial class ArtistService
  {
    public async Task<List<ArtistResponse>> ArtistProfilesAsync() {
      return await _db.Artists.Select(artist => new ArtistResponse {
        Name = artist.Name,
        AudioCount = artist.Audios == null ? 0 : artist.Audios.Count(),
        Nationality = artist.Nationality,
        InActivity = artist.InActivity,
        Note = artist.Note,
        MostFrequentTags = artist.Audios == null ? null :
          artist.Audios.SelectMany(audio => audio.Metadata.Tags)
          .GroupBy(tag => tag.Name)
          .OrderByDescending(group => group.Count())
          .Take(3)
          .Select(group => new TagFrequency(group.Key, group.Count()))
          .ToList()
      }).ToListAsync();
    }

    public async Task<ArtistResponse> ArtistProfileAsync(Guid artistId) {
      return await _db.Artists
          .Where(a => a.Id == artistId)
          .Select(artist => new ArtistResponse {
            Name = artist.Name,
            AudioCount = artist.Audios == null ? 0 : artist.Audios.Count(),
            Nationality = artist.Nationality,
            InActivity = artist.InActivity,
            Note = artist.Note,
            MostFrequentTags = artist.Audios == null ? null :
              artist.Audios.SelectMany(audio => audio.Metadata.Tags)
              .GroupBy(tag => tag.Name)
              .OrderByDescending(group => group.Count())
              .Take(3)
              .Select(group => new TagFrequency(group.Key, group.Count()))
              .ToList()
          }).FirstAsync();
    }

    public async Task<ArtistResponse> ArtistProfileByNameAsync(string name) {
      return await _db.Artists.Select(artist => new ArtistResponse {
        Name = artist.Name,
        AudioCount = artist.Audios == null ? 0 : artist.Audios.Count(),
        Nationality = artist.Nationality,
        InActivity = artist.InActivity,
        Note = artist.Note,
        MostFrequentTags = artist.Audios == null ? null :
          artist.Audios.SelectMany(audio => audio.Metadata.Tags)
          .GroupBy(tag => tag.Name)
          .OrderByDescending(group => group.Count())
          .Take(3)
          .Select(group => new TagFrequency(group.Key, group.Count()))
          .ToList()
      }).Where(a => a.Name == name).FirstAsync();
    }
  }
}

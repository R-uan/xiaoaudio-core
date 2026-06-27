using AudioArchive.Database.Entity;
using AudioArchive.Modules.Artists.Requests;
using AudioArchive.Modules.Artists.Responses;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Artists.Services
{
  public partial class ArtistService
  {
    public async Task<ArtistResponse> InsertArtistAsync(PostArtistRequest request) {
      var artist = Artist.From(request);
      await _db.Artists.AddAsync(artist);
      await _db.SaveChangesAsync();
      return ArtistResponse.From(artist);
    }

    public async Task<ArtistResponse> UpdateArtistAsync(Guid artistId, PatchArtistRequest request) {
      var artist = await _db.Artists.Where(a => a.Id == artistId).FirstAsync();
      if (!string.IsNullOrEmpty(request.Name)) artist.Name = request.Name;
      if (!string.IsNullOrEmpty(request.Nationality)) artist.Nationality = request.Nationality;
      if (!string.IsNullOrEmpty(request.Note)) artist.Note = request.Note;
      if (!string.IsNullOrEmpty(request.BasedAt)) artist.BasedAt = request.BasedAt;
      if (!string.IsNullOrEmpty(request.Biography)) artist.Biography = request.Biography;
      if (request.Birthday.HasValue) artist.Birthday = request.Birthday;
      if (request.InActivity.HasValue) artist.InActivity = request.InActivity.Value;
      if (request.DebutDate.HasValue) artist.DebutDate = request.DebutDate;
      if (request.GraduationDate.HasValue) artist.GraduationDate = request.GraduationDate;
      await _db.SaveChangesAsync();
      return ArtistResponse.From(artist);
    }
  }
}

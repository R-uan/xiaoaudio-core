using AudioArchive.Database;
using AudioArchive.Modules.Artists.Requests;
using AudioArchive.Modules.Artists.Responses;

namespace AudioArchive.Modules.Artists.Services
{
  public interface IArtistService
  {
    // Public
    Task<List<ArtistResponse>> ArtistProfilesAsync();
    Task<ArtistResponse> ArtistProfileAsync(Guid artistId);
    Task<ArtistResponse> ArtistProfileByNameAsync(string name);
    // Internal
    Task<ArtistResponse> UpdateArtistAsync(Guid artistId, PatchArtistRequest request);
    Task<ArtistResponse> InsertArtistAsync(PostArtistRequest request);
  }

  public partial class ArtistService(DatabaseContext databaseContext) : IArtistService
  {
    private readonly DatabaseContext _db = databaseContext;
  }
}

using AudioArchive.Modules.Artists.Requests;
using AudioArchive.Modules.Artists.Responses.Views;

namespace AudioArchive.Modules.Artists.Services
{
  public interface IArtistService
  {
    public Task<List<ArtistProfileView>> ArtistProfiles();
    public Task<ArtistProfileView> ArtistProfile(Guid artistId);
    public Task<ArtistProfileView> ArtistProfileByNameAync(string name);
    public Task<ArtistProfileView> UpdateArtist(Guid artistId, PatchArtistRequest request);
  }
}

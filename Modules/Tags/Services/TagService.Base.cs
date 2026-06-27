using AudioArchive.Database;
using AudioArchive.Modules.Tags.Requests;
using AudioArchive.Modules.Tags.Responses;

namespace AudioArchive.Modules.Tags.Services
{
  public interface ITagService
  {
    // Public
    Task<List<TagResponse>> GetTagsAsync();
    // Internal
    Task<TagResponse> UpdateTagAsync(Guid tagId, PatchTagRequest request);
    Task DeleteTagAsync(Guid tagId);
  }

  public partial class TagService(DatabaseContext databaseContext) : ITagService
  {
    private readonly DatabaseContext _db = databaseContext;
  }
}

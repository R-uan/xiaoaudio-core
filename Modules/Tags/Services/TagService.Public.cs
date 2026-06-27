using AudioArchive.Modules.Tags.Responses;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Modules.Tags.Services
{
  public partial class TagService
  {
    public async Task<List<TagResponse>> GetTagsAsync() {
      return await _db.Tags
        .Select(t => new TagResponse {
          Id = t.Id,
          Name = t.Name,
          Description = t.Description,
          AudioCount = t.AudioMetadatas == null ? 0 : t.AudioMetadatas.Count,
        })
        .OrderBy(t => t.Name)
        .ToListAsync();
    }
  }
}

using AudioArchive.Database.Entity;
using AudioArchive.Modules.Tags.Requests;
using AudioArchive.Modules.Tags.Responses;
using AudioArchive.Shared;

namespace AudioArchive.Modules.Tags.Services
{
  public partial class TagService
  {
    public async Task<TagResponse> UpdateTagAsync(Guid tagId, PatchTagRequest request) {
      var tag = await _db.Tags.FindAsync(tagId) ??
        throw new NotFoundException(
          Message: "Could not find tag entry.",
          Target: tagId.ToString()
        );

      if (!string.IsNullOrEmpty(request.Name)) tag.Name = request.Name.Trim();
      if (!string.IsNullOrEmpty(request.Description)) tag.Description = request.Description.Trim();

      await _db.SaveChangesAsync();

      return new TagResponse {
        Id = tag.Id,
        Name = tag.Name,
        Description = tag.Description,
        AudioCount = tag.AudioMetadatas == null ? 0 : tag.AudioMetadatas.Count,
      };
    }

    public async Task DeleteTagAsync(Guid tagId) {
      var tag = await _db.Tags.FindAsync(tagId) ??
        throw new NotFoundException(
          Message: "Could not find tag entry.",
          Target: tagId.ToString()
        );

      _db.Tags.Remove(tag);
      await _db.SaveChangesAsync();
    }
  }
}

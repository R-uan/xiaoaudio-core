using Microsoft.AspNetCore.Mvc;

namespace AudioArchive.Modules.Tags.Controllers
{
  public partial class TagController
  {
    [HttpGet]
    public async Task<IActionResult> GetTags() {
      var tags = await _tagService.GetTagsAsync();
      return Ok(new {
        tags.Count,
        Data = tags
      });
    }
  }
}

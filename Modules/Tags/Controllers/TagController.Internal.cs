using AudioArchive.Modules.Tags.Requests;
using AudioArchive.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioArchive.Modules.Tags.Controllers
{
  public partial class TagController
  {
    [HttpDelete("{tagId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTag([FromRoute] string tagId) {
      if (!Guid.TryParse(tagId, out var tagGuid))
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: tagId
        );

      await _tagService.DeleteTagAsync(tagGuid);

      return Ok(new {
        Message = "Tag successfully deleted.",
        Target = tagId
      });
    }

    [HttpPatch("{tagId}")]
    [Authorize]
    public async Task<IActionResult> UpdateTag([FromRoute] string tagId, [FromBody] PatchTagRequest body) {
      if (!Guid.TryParse(tagId, out var tagGuid))
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: tagId
        );

      var result = await _tagService.UpdateTagAsync(tagGuid, body);

      return Ok(new { result.Name, result.Description });
    }
  }
}

using AudioArchive.Database;
using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Tags.Services;

namespace AudioArchive.Modules.Tags.Controllers
{
  [ApiController]
  [Route("api/tag")]
  public partial class TagController(
    ITagService tagService,
    DatabaseContext databaseContext
  ) : ControllerBase
  {
    private readonly ITagService _tagService = tagService;
    private readonly DatabaseContext _databaseContext = databaseContext;
  }
}

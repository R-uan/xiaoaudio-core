using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Requests;
using Microsoft.AspNetCore.Authorization;

namespace AudioArchive.Modules.Core.Controllers
{
  public partial class AccountController
  {
    [Authorize]
    [HttpPost("password-change")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeRequest req) {
      await _accountService.PasswordChangeAsync(req);
      return Ok(new { Success = true });
    }
  }
}

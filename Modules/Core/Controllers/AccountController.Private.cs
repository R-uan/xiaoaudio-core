using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AudioArchive.Modules.Core.Controllers
{
  public partial class AccountController
  {
    [Authorize]
    [HttpGet("verify")]
    public async Task<IActionResult> VerifyAccount([FromQuery] string? code = null) {
      var verified = await _accountService.AccountVerificationAsync(code);
      return this.Ok(new { Success = verified });
    }
  }
}

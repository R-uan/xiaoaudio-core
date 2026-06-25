using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Services;

namespace AudioArchive.Modules.Core.Controllers {
  [ApiController]
  [Route("api/account")]
  public partial class AccountController(IAccountService accountService) : ControllerBase {
    private readonly IAccountService _accountService = accountService;

    [HttpPost("authentication")]
    public async Task<IActionResult> AuthenticateAccount([FromBody] AuthenticationRequest request) {
      var token = await this._accountService.AuthenticateAccountAsync(request);
      return Ok(new { Success = true, Data = new { Token = token } });
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request) {
      var account = this._accountService.RegisterAccountAsync(request);
      return Ok(new {
        Success = true,
        Date = account
      });
    }

    [HttpPost("verification")]
    public async Task<IActionResult> VerifyUnique([FromQuery] string type, [FromQuery] string value) {
      if (type.ToLower() == "email") {
        return Ok(new {
          Success = true,
          Available = this._accountService.VerifyEmailAvailabilityAsync(value),
        });
      } else if (type.ToLower() == "username") {
        return Ok(new {
          Success = true,
          Available = this._accountService.VerifyUsernameAvailabilityAsync(value),
        });
      } else {
        return BadRequest(new {
          Success = false,
          Message = "Invalid value."
        });
      }
    }
  }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AudioArchive.Modules.Support.Requests;

namespace AudioArchive.Modules.Support.Controllers
{
  public partial class SupportController : ControllerBase
  {
    [Authorize]
    [HttpPost("ticket")]
    public async Task<IActionResult> OpenTicket([FromBody] CreateTicketRequest request) {
      var ticket = await _supportService.OpenTicketAsync(request);
      return Ok(new { Success = true, Data = ticket });
    }

    [Authorize]
    [HttpPost("ticket/message")]
    public async Task<IActionResult> SendMessage([FromBody] SendTicketMessageRequest request) {
      await _supportService.SendTicketMessageAsync(request);
      return Ok(new { Success = true });
    }
  }
}

using AudioArchive.Shared;
using AudioArchive.Database;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Support.Requests;

namespace AudioArchive.Modules.Support.Services
{
  public class SupportService(IHttpContextAccessor httpContext, DatabaseContext database) : ISupportService
  {
    private readonly DatabaseContext _db = database;
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private Account CurrentUser => _httpContext.HttpContext?.Items["CurrentUser"] as Account
      ?? throw new UnauthorizedAccessException("Not authenticated");

    public async Task<SupportTicket> OpenTicket(CreateTicketRequest request) {
      var user = await this._db.Accounts.Include(u => u.RequestedTickets)
        .FirstOrDefaultAsync(u => u.Id == CurrentUser.Id)
        ?? throw new NotFoundException(
          Message: "User not found",
          Target: "SupportService::CreateNewTicket"
        );

      if (user.RequestedTickets?.Count > 10) {
        throw new SupportException(
          Message: "Maximum number of tickets reached.",
          Target: "SupportService::CreateNewTicket"
        );
      }

      var ticket = new SupportTicket {
        Message = request.InitialMessage,
        Status = TicketStatus.Open,
        Type = request.TicketType,
        RequesterId = user.Id,
        Requester = user,
      };

      await this._db.SupportTickets.AddAsync(ticket);
      return ticket;
    }

    public async Task SendTicketMessage(SendTicketMessageRequest request) {
      var ticket = await this._db.SupportTickets.FindAsync(request.TicketId)
        ?? throw new NotFoundException(
          Message: "Ticket not found.",
          Target: "SupportService::SendTicketMessage"
        );

      if (ticket.Requester != this.CurrentUser) {
        throw new UnauthorizedException(
          Message: "You do not own this ticket.",
          Target: "SupportService::SendTicketMessage"
        );
      }

      var message = new SupportTicketMessage {
        Content = request.Message,
        User = this.CurrentUser,
        UserId = this.CurrentUser.Id,
        SupportTicket = ticket,
        SupportTicketId = ticket.Id
      };

      await this._db.SupportTicketMessages.AddAsync(message);
    }
  }
}

using AudioArchive.Shared;
using AudioArchive.Database;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Support.Requests;

namespace AudioArchive.Modules.Support.Services
{
  public partial class SupportService
  {
    public async Task<SupportTicket> OpenTicketAsync(CreateTicketRequest request) {
      var currentUser = await _currentAccount.GetAsync();

      var user = await this._db.Accounts.Include(u => u.RequestedTickets)
        .FirstOrDefaultAsync(u => u.Id == currentUser.Id)
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

    public async Task SendTicketMessageAsync(SendTicketMessageRequest request) {
      var currentUser = await _currentAccount.GetAsync();

      var ticket = await this._db.SupportTickets.FindAsync(request.TicketId)
        ?? throw new NotFoundException(
          Message: "Ticket not found.",
          Target: "SupportService::SendTicketMessage"
        );

      if (ticket.Requester != currentUser) {
        throw new UnauthorizedException(
          Message: "You do not own this ticket.",
          Target: "SupportService::SendTicketMessage"
        );
      }

      var message = new SupportTicketMessage {
        Content = request.Message,
        User = currentUser,
        UserId = currentUser.Id,
        SupportTicket = ticket,
        SupportTicketId = ticket.Id
      };

      await this._db.SupportTicketMessages.AddAsync(message);
    }
  }
}

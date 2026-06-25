using AudioArchive.Modules.Support.Requests;
using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Support.Services
{
  public interface ISupportService {
    Task<SupportTicket> OpenTicket(CreateTicketRequest request);
    Task SendTicketMessage(SendTicketMessageRequest request);
  }
}

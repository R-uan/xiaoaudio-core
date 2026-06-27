using AudioArchive.Database;
using AudioArchive.Infrastructure.Identity;
using AudioArchive.Modules.Support.Requests;
using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Support.Services
{
  public interface ISupportService
  {
    Task<SupportTicket> OpenTicketAsync(CreateTicketRequest request);
    Task SendTicketMessageAsync(SendTicketMessageRequest request);
  }

  public partial class SupportService(
    DatabaseContext database,
    ICurrentAccount currentAccount
  ) : ISupportService
  {
    private readonly DatabaseContext _db = database;
    private readonly ICurrentAccount _currentAccount = currentAccount;
  }
}

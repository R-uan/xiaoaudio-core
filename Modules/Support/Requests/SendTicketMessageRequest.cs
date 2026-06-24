using AudioArchive.Database.Entity;
namespace AudioArchive.Modules.Support.Requests
{
  public class SendTicketMessageRequest
  {
    public int TicketId { get; set; }
    public required string Message { get; set; }
  }
}

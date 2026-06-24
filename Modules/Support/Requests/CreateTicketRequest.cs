using AudioArchive.Database.Entity;
namespace AudioArchive.Modules.Support.Requests
{
  public class CreateTicketRequest
  {
    public TicketType TicketType { get; set; }
    public required string InitialMessage { get; set; }
  }
}

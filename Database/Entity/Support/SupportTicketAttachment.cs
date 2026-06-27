namespace AudioArchive.Database.Entity
{
  public class SupportTicketAttachment
  {
    public required int Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public required int SupportTicketId { get; set; }
    public required SupportTicket SupportTicket { get; set; }
  }
}

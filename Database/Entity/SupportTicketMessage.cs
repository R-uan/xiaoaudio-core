namespace AudioArchive.Database.Entity
{
  public class SupportTicketMessage
  {
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public required Account User { get; set; }
    public required int UserId { get; set; }

    public required int SupportTicketId { get; set; }
    public required SupportTicket SupportTicket { get; set; }
  }
}

using System.Text.Json.Serialization;

namespace AudioArchive.Database.Entity
{
  public class SupportTicket
  {
    public int Id { get; set; }
    public required string Message { get; set; }
    public required TicketType Type { get; set; }
    public required TicketStatus Status { get; set; } = TicketStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastInteraction { get; set; } = DateTime.Now;

    [JsonIgnore] public Account? Representative { get; set; }
    [JsonIgnore] public int? RepresentativeId { get; set; }

    [JsonIgnore] public required Account Requester { get; set; }
    [JsonIgnore] public required int RequesterId { get; set; }

    public List<SupportTicketMessage>? Messages { get; set; }
    public List<SupportTicketAttachment>? Attachments { get; set; }
  }

  public enum TicketType
  {
    Reports,
    Account,
    Feedback,
    Copyright,
    Verification,
  }

  public enum TicketStatus
  {
    Open,
    Pending,
    InProgress,
    OnHold,
    Resolved,
    Closed,
    Reopened,
    Cancelled,
    Duplicate
  }
}

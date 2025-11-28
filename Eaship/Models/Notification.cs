public class Notification
{
    public long NotificationId { get; set; }
    public long UserId { get; set; }
    public string Type { get; set; } = "";
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";

    public long? BookingId { get; set; }
    public long? ContractId { get; set; }
    public long? CompanyId { get; set; }

    public DateTime CreatedAt { get; set; }
}

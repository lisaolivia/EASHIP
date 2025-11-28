using Eaship.Models;

public interface INotificationService
{
    void Create(
        long userId,
        string type,
        string title,
        string message,
        long? bookingId = null,
        long? contractId = null,
        long? companyId = null
    );

    List<Notification> GetAll(long userId);   // <--- TAMBAHKAN INI
}


public class NotificationService : INotificationService
{
    private readonly EashipDbContext _context;

    public List<Notification> GetAll(long userId)
    {
        return _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
    }


    public NotificationService(EashipDbContext context)
    {
        _context = context;
    }

    public void Create(
        long userId,
        string type,
        string title,
        string message,
        long? bookingId = null,
        long? contractId = null,
        long? companyId = null
    )
    {
        var notif = new Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            BookingId = bookingId,
            ContractId = contractId,
            CompanyId = companyId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notif);
        _context.SaveChanges();
    }
}

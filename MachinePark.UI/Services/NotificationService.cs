namespace MachinePark.UI.Services;

public interface INotificationService
{
    event Action Notify;
    void DispatchNotification();
}

public class NotificationService : INotificationService
{
    public event Action Notify;

    public void DispatchNotification()
    {
        Notify?.Invoke();
    }
}
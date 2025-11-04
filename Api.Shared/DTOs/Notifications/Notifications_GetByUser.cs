
public class Notifications_GetByUser
{
    public int NotificationId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Url { get; set; }

    public bool? Viwed { get; set; }
}

public class Notifications_SendModel
{
    public int NotificationId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Url { get; set; }
}

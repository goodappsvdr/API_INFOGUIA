namespace Api.Infrastructure.SignalR
{
    /// <summary>
    /// NotificationHub
    /// </summary>
    public class NotificationHub : Hub
    {
        //Envia un mensaje al usuario que esta conectado
        public async Task SendMessage(string UserId, string Message)
        {
            var groupName = $"user-{UserId}";
            await Clients.Group(groupName).SendAsync("SendUserMessageSuccess", Message);
        }

        // Envia una notificacion al usuario que esta conectado
        public async Task SendNotification(Notifications_SendModel Model, string UserId)
        {
            var groupName = $"user-{UserId}";
            await Clients.Group(groupName).SendAsync("SendNotificationSuccess", Model);
        }

        //Cuando se conecta un usuario
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                var groupName = $"user-{userId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnConnectedAsync();
        }

        // cuando se desconecta un usuario
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

    }
}

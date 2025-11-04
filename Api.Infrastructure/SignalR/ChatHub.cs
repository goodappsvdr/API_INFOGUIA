using Microsoft.AspNetCore.SignalR;

namespace Api.Infrastructure.SignalR
{

    /// <summary>
    /// ChatHub
    /// </summary>
    //public class ChatHub : Hub
    //{
    //    private readonly Context _context;
    //    private readonly NotificationHub _notificationHub;
    //    private readonly IChatsServices _chatsServices;

    //    /// <summary>
    //    /// Constructor
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="notificationHub"></param>
    //    public ChatHub(Context context, NotificationHub notificationHub, IChatsServices chatsServices)
    //    {
    //        _context = context;
    //        _notificationHub = notificationHub;
    //        _chatsServices = chatsServices;
    //    }

    //    /// <summary>
    //    /// Entra al grupo
    //    /// </summary>
    //    /// <param name="IdChat"></param>
    //    /// <param name="IdUser"></param>
    //    /// <returns></returns>
    //    public async Task JoinGroup(string IdChat, string IdUser)
    //    {
    //        if (!await _context.ChatsUsers.AnyAsync(x => x.IdChat == IdChat))
    //        {
    //            await Clients.Caller.SendAsync("ReceiveError", "El Grupo que estas buscando no existe");
    //        }
    //        else
    //        {
    //            var ChatUser = await _context.ChatsUsers.Where(x => x.IdChat == IdChat).Select(x => new { x.IdUserPayed, x.IdUserClient }).FirstOrDefaultAsync();

    //            if (ChatUser != null)
    //            {
    //                if (ChatUser.IdUserPayed == IdUser)
    //                {
    //                    await Groups.AddToGroupAsync(Context.ConnectionId, IdChat);
    //                }
    //                else if (ChatUser.IdUserClient == IdUser)
    //                {
    //                    await Groups.AddToGroupAsync(Context.ConnectionId, IdChat);
    //                }
    //                else
    //                {
    //                    await Clients.Caller.SendAsync("ReceiveError", "El usuario que esta intentando entrar no pertenece al grupo");
    //                }

    //            }
    //        }

    //    }

    //    /// <summary>
    //    /// Sale del grupo
    //    /// </summary>
    //    /// <param name="IdChat"></param>
    //    /// <param name="IdUser"></param>
    //    /// <returns></returns>
    //    public async Task LeaveGroup(string IdChat, string IdUser)
    //    {
    //        if (!await _context.ChatsUsers.AnyAsync(x => x.IdChat == IdChat))
    //        {
    //            await Clients.Caller.SendAsync("ReceiveError", "El Grupo que estas buscando no existe");
    //        }
    //        else
    //        {
    //            var ChatUser = await _context.ChatsUsers.Where(x => x.IdChat == IdChat).Select(x => new { x.IdUserPayed, x.IdUserClient }).FirstOrDefaultAsync();

    //            if (ChatUser != null)
    //            {
                   
    //                if (ChatUser.IdUserPayed == IdUser)
    //                {
    //                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, IdChat);
    //                }
    //                else if (ChatUser.IdUserClient == IdUser)
    //                {
    //                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, IdChat);
    //                }
    //                else
    //                {
    //                    await Clients.Caller.SendAsync("ReceiveError", "El usuario que esta intentando salir no pertenece al grupo");
    //                }

    //            }
    //        }

    //    }

    //    /// <summary>
    //    /// Enviar mensajes al grupo
    //    /// </summary>
    //    /// <param name="IdChat"></param>
    //    /// <param name="message"></param>
    //    /// <param name="IdUser"></param>
    //    /// <returns></returns>
    //    public async Task SendMessageToGroup(string IdChat, string message, string IdUser)
    //    {
    //        try
    //        {
    //            string Username = await _context.AspNetUsers.Where(x => x.Id == IdUser).Select(x => x.UserName).FirstOrDefaultAsync();

    //            var IdOtherUser = await _context.ChatsUsers
    //                    .Where(chat => chat.IdChat == IdChat && (chat.IdUserPayed == IdUser || chat.IdUserClient == IdUser))
    //                    .Select(chat =>
    //                        chat.IdUserPayed == IdUser ? chat.IdUserClient : chat.IdUserPayed
    //                    )
    //                    .FirstOrDefaultAsync();

    //            var Name = await _context.AspNetUsers.Where(x => x.Id == IdOtherUser).Select(x => x.UserName).FirstOrDefaultAsync();

    //            bool Active = await _context.AspNetUsersProfiles.Where(x => x.IdUser == IdOtherUser).Select(x => x.Active.Value).FirstOrDefaultAsync();
    //            if (!Active)
    //            {
    //                await _notificationHub.SendMessage(IdUser, $"El usuario {Name} desabilito su cuenta, no puede recibir mensajes.");
    //            }
    //            else
    //            {
    //                Chat Data = new()
    //                {
    //                    MessageDate = DateTimeOffset.UtcNow,
    //                    Message = message,
    //                    UserName = Username,
    //                    IdUser = IdUser,
    //                    IdUserChat = IdChat,
    //                    IdStatus = 1,
    //                    Viewed = false,
    //                    IdUserReceiver = IdOtherUser,
    //                    UserNameReceiver = Name,
    //                };

    //                await _context.Chats.AddAsync(Data);
    //                await _context.SaveChangesAsync();
    //                await Clients.Group(IdChat).SendAsync("ReceiveMessage", Data);

    //                List<ListChatsModel> ListChats = await _chatsServices.GetAsync(IdOtherUser);
    //                ListChatsModel Chat = ListChats.Where(x => x.ChatId == IdChat).FirstOrDefault();

    //                await _notificationHub.SendUserMessage(IdOtherUser, Chat);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await Clients.Caller.SendAsync("ReceiveError", ex.Message);
    //        }
    //    }

    //    /// <summary>
    //    /// Lee un unico mensaje
    //    /// </summary>
    //    /// <param name="IdChat"></param>
    //    /// <param name="Id"></param>
    //    /// <returns></returns>
    //    public async Task ReadMessageOnly(string IdChat, int Id)
    //    {

    //        Chat mensaje = await _context.Chats.Where(m => m.Id == Id).FirstOrDefaultAsync();

    //        if (mensaje != null)
    //        {
    //            mensaje.Viewed = true;
    //            _context.Update(mensaje);
    //            await _context.SaveChangesAsync();

    //            await Clients.Group(IdChat).SendAsync("ReadMessageOnly", mensaje.Id);
    //        }

    //        await Clients.Group(IdChat).SendAsync("ReceiveError", "este usuario es el mismo que lo envio");
    //    }

    //    /// <summary>
    //    /// Lee todos los mensajes
    //    /// </summary>
    //    /// <param name="IdUser"></param>
    //    /// <param name="IdChat"></param>
    //    /// <returns></returns>
    //    public async Task ReadMessageAll(string IdUser, string IdChat)
    //    {

    //        var viweds = await _context.Chats.Where(x => x.IdUserChat == IdChat && x.IdUser != IdUser && x.Viewed == false).ToListAsync();

    //        foreach (var Viwed in viweds)
    //        {
    //            Viwed.Viewed = true;
    //            _context.Update(Viwed);
    //            await _context.SaveChangesAsync();
    //        }

    //        await Clients.Group(IdChat).SendAsync("ReadMessageAll", IdUser);
    //    }
    //}
}

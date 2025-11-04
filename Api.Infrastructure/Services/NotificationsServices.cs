using Api.Shared.Interface;
using AutoMapper;

namespace Api.Infrastructure.Services
{
    public class NotificationsServices : INotificationsServices
    {
        private readonly IRepository<Notification> _repository;
        private readonly IMapper _mapper;
        private readonly HubConnection _connection;
        public NotificationsServices(IRepository<Notification> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _connection = new HubConnectionBuilder().WithUrl($"https://api.allmarket360.com.ar/notificationHub").Build();
            _connection.StartAsync().Wait();
        }

        // Marca como visto a todos los mensajes
        public async Task<bool> Viwed(string UserId)
        {
            IQueryable<Notification> Data = await _repository.GetAllAsync(x => x.UserId == UserId && x.Viwed == false);
            Data.ToList().ForEach(x => x.Viwed = true);
            return await _repository.UpdateRange(Data);
        }

        // Obtiene una lista de notificaciones 
        public async Task<List<Notifications_GetByUser>> GetByUserId(string UserId)
        {
            IQueryable<Notification> ListNotification = await _repository.GetAllAsync(x => x.UserId == UserId && x.Delete == false);
            return _mapper.Map<List<Notifications_GetByUser>>(ListNotification);
        }

        // Obtiene una notificacion por su id
        public async Task<Notifications_GetByUser> GetByNotificationId(int NotificationId) => _mapper.Map<Notifications_GetByUser>(await _repository.GetAsync(x => x.Id == NotificationId));  
        
        // Agrega una nueva notificacion
        public async Task<Notification> AddAsync(Notification Model) => await _repository.AddAsync(Model);

        // Modifica una notificavion existente
        public async Task<bool> UpdateAsync(Notification Model) => await _repository.UpdateAsync(Model);
        
        // Elimina una notificacion
        public async Task<bool> DeleteAsync(int NotificationId)
        {
            Notification Data = await _repository.GetAsync(x => x.Id == NotificationId);
            if(Data == null) return false;
            Data.Delete = true;
            return await _repository.UpdateAsync(Data);
        }
            
        // Envia una notificacion a un usuario atravez de SignalR
        public async Task<int> SendNotification(Notification Model)
        {
            Notifications_SendModel SendModel = _mapper.Map<Notifications_SendModel>(Model);

            if (_connection.State != HubConnectionState.Connected)
                await _connection.StartAsync();

            if (_connection.State == HubConnectionState.Connected)
                await _connection.InvokeAsync("SendNotification", SendModel, Model.UserId);

            return 1;
        }
    }
}

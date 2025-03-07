using Microsoft.AspNetCore.SignalR;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Services;

namespace ParcelPointApi.Hubs
{
    public class HomeHub : BaseHub
    {
        public HomeHub(UserConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public class CombinedTableDTO
        {
            public ParcelLog Parcel { get; set; }
            public NotificationLog Notification { get; set; }
        }

        public async Task SendNewNotificationAndParcel(Guid userID, CombinedTableDTO homeUpdate)
        {
            var connection = _connectionManager.GetConnections(userID);
            foreach (var connectionId in connection)
            {
                await Clients.Client(connectionId).SendAsync("HomeListUpdate", homeUpdate);
            }
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Services;

namespace ParcelPointApi.Hubs
{
    public class UsersHub : BaseHub
    {
        public UsersHub(UserConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public async Task SendNewRegisteredUser(Guid userID, User user)
        {
            var connection = _connectionManager.GetConnections(userID);
            foreach (var connectionId in connection)
            {
                await Clients.Client(connectionId).SendAsync("UserListUpdate", user);
            }
        }
    }
}

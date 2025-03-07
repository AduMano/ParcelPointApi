using Microsoft.AspNetCore.SignalR;
using ParcelPointApi.Services;
using System.Threading.Tasks;

namespace ParcelPointApi.Hubs
{
    public class ParcelHub : BaseHub
    {
        public ParcelHub(UserConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public async Task SendNewDeliveredParcel(Guid userID, ParcelLog parcelItem)
        {
            var connection = _connectionManager.GetConnections(userID);
            foreach (var connectionId in connection)
            {
                await Clients.Client(connectionId).SendAsync("ParcelListUpdate", parcelItem);
            }
        }
    }
}
